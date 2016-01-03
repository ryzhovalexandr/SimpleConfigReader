# SimpleConfigReader
The simple tool for easy reading .NET configuration file (*web.config*, *app.config*) into the immutable class of settings.

## Get started
First you should define immutable class of settings.
This class should have a single constructor with parameters named like the parameters in configuration file.
```c#
    public class UserSettings
    {
        public UserSettings(string login, string password, int age, bool needConfirmation)
        {
            Login = login;
            Password = password;
            Age = age;
        }

        public string Login { get; private set; }

        public string Password { get; private set; }

        public int Age { get; private set; }
        
        public bool NeedConfirmation { get; private set; }
    }
```

*app.config* file:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="Login" value="admin" />
    <add key="Password" value="passw0rd" />
    <add key="Age" value="23" />
  </appSettings>
</configuration>
```
And then you simply read the settings from configuration file into object.
```c#
    var userSettings = AppConfigReader.ReadFromAppSettings<UserSettings>();
    
    loginService.Login(userSettings);
```
## Default values
You can define default values in constructor using nullable types.
```c#
    public class UserSettings
    {
        public UserSettings(string login, string password, int? age, bool? needConfirmation, int? timeoutInSeconds)
        {
            Login = string.IsNullOrEmpty(login) ? "mrBond" : login;
            Password = password;
            Age = age;
            NeedConfirmation = needConfirmation ?? true;
            TimeoutInSeconds = timeoutInSeconds ?? 10;
        }

        public string Login { get; private set; }

        public string Password { get; private set; }

        public int? Age { get; private set; }

        public bool NeedConfirmation { get; private set; }

        public int TimeoutInSeconds { get; private set; }
    }
```

If default value is not defined in constructor of settings class and there's no value for this parameter in configuration file, then we get default value for this type.
For example, for *int* type we'll get 0, and for *bool* - false.

## Reading array values
You can read array values separated by semicolon.

*app.config* file:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="Numbers" value="1;234;-23;45" />
  </appSettings>
</configuration>
```
```c#
    public class UserSettings
    {
        public UserSettings(IEnumerable<int> numbers)
        {
            Numbers = numbers;
        }

        public IEnumerable<int> Numbers { get; private set; }
    }
```

## Reading settings from custom section
You can write key-value settings in custom section.

*app.config* file:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
  </appSettings>
  <user>
    <add key="Login" value="admin" />
    <add key="Password" value="passw0rd" />
    <add key="Age" value="23" />
  </user>
</configuration>
```
For read this settings into object use another method:

```c#
    var userSettings = AppConfigReader.ReadFromSection<UserSettings>("user");
```

## Reading values from key-value collection.
If you have any settings, presented in key-value collection, you can read them into settings object. It's comfortable for reading settings from custom data sources.
```c#
    var collection = new KeyValueConfigurationCollection()
    {
        new KeyValueConfigurationElement("Login", "mrBond"), 
        new KeyValueConfigurationElement("Password", "testtest"),
        new KeyValueConfigurationElement("Age", "7")
    };

    var userSettings = ConfigurationReader<UserSettings>.ReadFromCollection(collection);
```

## Supported types
The parameters of constructor in settings class could be:
 - string
 - int
 - double
 - bool
 
and *IEnumerable<>* of this types.
