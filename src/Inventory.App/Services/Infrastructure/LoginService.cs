#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Threading.Tasks;

using Windows.Storage.Streams;
using Windows.Security.Credentials;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Inventory.Services
{
    public class LoginService : ILoginService
    {
        public LoginService(IMessageService messageService, IDialogService dialogService)
        {
            IsAuthenticated = false;
            MessageService = messageService;
            DialogService = dialogService;
        }

        public IMessageService MessageService { get; }
        public IDialogService DialogService { get; }

        public bool IsAuthenticated { get; set; }

        public bool IsWindowsHelloEnabled(string userName)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                if (userName.Equals(AppSettings.Current.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    return AppSettings.Current.WindowsHelloPublicKeyHint != null;
                }
            }
            return false;
        }

        public Task<bool> SignInWithPasswordAsync(string userName, string password)
        {
            // Perform authentication here.
            // This sample accepts any user name and password.
            UpdateAuthenticationStatus(true);
            return Task.FromResult(true);
        }

        public async Task<Result> SignInWithWindowsHelloAsync()
        {
            string userName = AppSettings.Current.UserName;
            if (IsWindowsHelloEnabled(userName))
            {
                var retrieveResult = await KeyCredentialManager.OpenAsync(userName);
                if (retrieveResult.Status == KeyCredentialStatus.Success)
                {
                    var credential = retrieveResult.Credential;
                    var challengeBuffer = CryptographicBuffer.DecodeFromBase64String("challenge");
                    var result = await credential.RequestSignAsync(challengeBuffer);
                    if (result.Status == KeyCredentialStatus.Success)
                    {
                        UpdateAuthenticationStatus(true);
                        return Result.Ok();
                    }
                    return Result.Error("Windows Hello", $"Cannot sign in with Windows Hello: {result.Status}");
                }
                return Result.Error("Windows Hello", $"Cannot sign in with Windows Hello: {retrieveResult.Status}");
            }
            return Result.Error("Windows Hello", "Windows Hello is not enabled for current user.");
        }

        public void Logoff()
        {
            UpdateAuthenticationStatus(false);
        }

        private void UpdateAuthenticationStatus(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            MessageService.Send(this, "AuthenticationChanged", IsAuthenticated);
        }

        public async Task TrySetupWindowsHelloAsync(string userName)
        {
            if (await KeyCredentialManager.IsSupportedAsync())
            {
                if (await DialogService.ShowAsync("Windows Hello", "Your device supports Windows Hello and you can use it to authenticate with the app.\r\n\r\nDo you want to enable Windows Hello for your next sign in with this user?", "Ok", "Maybe later"))
                {
                    await SetupWindowsHelloAsync(userName);
                }
                else
                {
                    await TryDeleteCredentialAccountAsync(userName);
                }
            }
        }

        private async Task SetupWindowsHelloAsync(string userName)
        {
            var publicKey = await CreatePassportKeyCredentialAsync(userName);
            if (publicKey != null)
            {
                if (await RegisterPassportCredentialWithServerAsync(publicKey))
                {
                    // When communicating with the server in the future, we pass a hash of the
                    // public key in order to identify which key the server should use to verify the challenge.
                    var hashProvider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
                    var publicKeyHash = hashProvider.HashData(publicKey);
                    AppSettings.Current.WindowsHelloPublicKeyHint = CryptographicBuffer.EncodeToBase64String(publicKeyHash);
                }
            }
            else
            {
                await TryDeleteCredentialAccountAsync(userName);
            }
        }

        private async Task<IBuffer> CreatePassportKeyCredentialAsync(string userName)
        {
            // Create a new KeyCredential for the user on the device
            var keyCreationResult = await KeyCredentialManager.RequestCreateAsync(userName, KeyCredentialCreationOption.ReplaceExisting);

            if (keyCreationResult.Status == KeyCredentialStatus.Success)
            {
                // User has autheniticated with Windows Hello and the key credential is created
                var credential = keyCreationResult.Credential;
                return credential.RetrievePublicKey();
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.NotFound)
            {
                await DialogService.ShowAsync("Windows Hello", "To proceed, Windows Hello needs to be configured in Windows Settings (Accounts -> Sign-in options)");
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.UnknownError)
            {
                await DialogService.ShowAsync("Windows Hello Error", "The key credential could not be created. Please try again.");
            }

            return null;
        }

        const int NTE_NO_KEY = unchecked((int)0x8009000D);
        const int NTE_PERM = unchecked((int)0x80090010);

        static private async Task<bool> TryDeleteCredentialAccountAsync(string userName)
        {
            try
            {
                AppSettings.Current.WindowsHelloPublicKeyHint = null;
                await KeyCredentialManager.DeleteAsync(userName);
                return true;
            }
            catch (Exception ex)
            {
                switch (ex.HResult)
                {
                    case NTE_NO_KEY:
                        // Key is already deleted. Ignore this error.
                        break;
                    case NTE_PERM:
                        // Access is denied. Ignore this error. We tried our best.
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        break;
                }
            }
            return false;
        }

        static private Task<bool> RegisterPassportCredentialWithServerAsync(IBuffer publicKey)
        {
            // TODO:
            // Register the public key and attestation of the key credential with the server
            // In a real-world scenario, this would likely also include:
            //      - Certificate chain for attestation endorsement if available
            //      - Status code of the Key Attestation result : Included / retrieved later / retry type
            return Task.FromResult(true);
        }
    }
}
