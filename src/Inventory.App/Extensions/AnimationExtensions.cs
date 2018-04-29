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
using System.Numerics;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;

using Microsoft.Graphics.Canvas.Effects;

namespace Inventory.Animations
{
    static public class AnimationExtensions
    {
        static public void Fade(this UIElement element, double milliseconds, double start, double end, CompositionEasingFunction easingFunction = null)
        {
            element.StartAnimation(nameof(Visual.Opacity), CreateScalarAnimation(milliseconds, start, end, easingFunction));
        }

        static public void TranslateX(this UIElement element, double milliseconds, double start, double end, CompositionEasingFunction easingFunction = null)
        {
            ElementCompositionPreview.SetIsTranslationEnabled(element, true);
            element.StartAnimation("Translation.X", CreateScalarAnimation(milliseconds, start, end, easingFunction));
        }

        static public void TranslateY(this UIElement element, double milliseconds, double start, double end, CompositionEasingFunction easingFunction = null)
        {
            ElementCompositionPreview.SetIsTranslationEnabled(element, true);
            element.StartAnimation("Translation.Y", CreateScalarAnimation(milliseconds, start, end, easingFunction));
        }

        static public void Scale(this FrameworkElement element, double milliseconds, double start, double end, CompositionEasingFunction easingFunction = null)
        {
            element.SetCenterPoint(element.ActualWidth / 2.0, element.ActualHeight / 2.0);
            var vectorStart = new Vector3((float)start, (float)start, 0);
            var vectorEnd = new Vector3((float)end, (float)end, 0);
            element.StartAnimation(nameof(Visual.Scale), CreateVector3Animation(milliseconds, vectorStart, vectorEnd, easingFunction));
        }

        static public void Blur(this UIElement element, double amount)
        {
            var brush = CreateBlurEffectBrush(amount);
            element.SetBrush(brush);
        }
        static public void Blur(this UIElement element, double milliseconds, double start, double end, CompositionEasingFunction easingFunction = null)
        {
            var brush = CreateBlurEffectBrush();
            element.SetBrush(brush);
            brush.StartAnimation("Blur.BlurAmount", CreateScalarAnimation(milliseconds, start, end, easingFunction));
        }

        static public void Grayscale(this UIElement element)
        {
            var brush = CreateGrayscaleEffectBrush();
            element.SetBrush(brush);
        }

        static public void SetBrush(this UIElement element, CompositionBrush brush)
        {
            var spriteVisual = CreateSpriteVisual(element);
            spriteVisual.Brush = brush;
            ElementCompositionPreview.SetElementChildVisual(element, spriteVisual);
        }

        static public void ClearEffects(this UIElement element)
        {
            ElementCompositionPreview.SetElementChildVisual(element, null);
        }

        static public SpriteVisual CreateSpriteVisual(UIElement element)
        {
            return CreateSpriteVisual(ElementCompositionPreview.GetElementVisual(element));
        }
        static public SpriteVisual CreateSpriteVisual(Visual elementVisual)
        {
            var compositor = elementVisual.Compositor;
            var spriteVisual = compositor.CreateSpriteVisual();
            var expression = compositor.CreateExpressionAnimation();
            expression.Expression = "visual.Size";
            expression.SetReferenceParameter("visual", elementVisual);
            spriteVisual.StartAnimation(nameof(Visual.Size), expression);
            return spriteVisual;
        }

        static public void SetCenterPoint(this UIElement element, double x, double y)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            visual.CenterPoint = new Vector3((float)x, (float)y, 0);
        }

        static public void StartAnimation(this UIElement element, string propertyName, CompositionAnimation animation)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            visual.StartAnimation(propertyName, animation);
        }

        static public CompositionAnimation CreateScalarAnimation(double milliseconds, double start, double end, CompositionEasingFunction easingFunction = null)
        {
            var animation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
            animation.InsertKeyFrame(0.0f, (float)start, easingFunction);
            animation.InsertKeyFrame(1.0f, (float)end, easingFunction);
            animation.Duration = TimeSpan.FromMilliseconds(milliseconds);
            return animation;
        }

        static public CompositionAnimation CreateVector3Animation(double milliseconds, Vector3 start, Vector3 end, CompositionEasingFunction easingFunction = null)
        {
            var animation = Window.Current.Compositor.CreateVector3KeyFrameAnimation();
            animation.InsertKeyFrame(0.0f, start);
            animation.InsertKeyFrame(1.0f, end);
            animation.Duration = TimeSpan.FromMilliseconds(milliseconds);
            return animation;
        }

        static public CompositionEffectBrush CreateBlurEffectBrush(double amount = 0.0)
        {
            var effect = new GaussianBlurEffect
            {
                Name = "Blur",
                BlurAmount = (float)amount,
                Source = new CompositionEffectSourceParameter("source")
            };

            var compositor = Window.Current.Compositor;
            var factory = compositor.CreateEffectFactory(effect, new[] { "Blur.BlurAmount" });
            var brush = factory.CreateBrush();
            brush.SetSourceParameter("source", compositor.CreateBackdropBrush());
            return brush;
        }

        static public CompositionEffectBrush CreateGrayscaleEffectBrush()
        {
            var effect = new GrayscaleEffect
            {
                Name = "Grayscale",
                Source = new CompositionEffectSourceParameter("source")
            };

            var compositor = Window.Current.Compositor;
            var factory = compositor.CreateEffectFactory(effect);
            var brush = factory.CreateBrush();
            brush.SetSourceParameter("source", compositor.CreateBackdropBrush());
            return brush;
        }
    }
}
