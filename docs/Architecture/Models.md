# Models
Models are another essential component in the MVVM architecture pattern. Model classes are non-visual classes that encapsulate the app's data. Therefore, the model can be thought as the representation of a domain object, which usually includes data and business validation logic.

In this application, a model wraps the data of a business object to better expose its properties to the view. In other words, the business object is the raw data received from the data source while the Model is a “view friendly” version, adapting or extending its properties for a better representation of the data.

For example, the Customer business object contains the properties “Name” and “Last Name” and the CustomerModel expose also the property “FullName” as a concatenation of these two properties to be used in the Customer details view.

In a more complex scenario, the Customer business object contains only the “CountryCode” while the CustomerModel also expose the “CountryName” property updated from a lookup table if the “CountryCode” changes if, for instance, the user select a new country in a ComboBox control.

The Model also helps decouple the business objects used in the data layer from the classes used in the presentation layer, so if a change is required in a business object schema, the application will be less affected.

All models inherit from the ObservableObject class. This class contains a typical implementation of the INotifyPropertyChanged interface, providing property change notifications, so an observer (usually the view) can update the representation of the model.

