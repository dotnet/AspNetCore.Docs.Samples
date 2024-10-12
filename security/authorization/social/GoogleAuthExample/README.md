# Google Authtication Example

### steps to integrate google login to your asp.net app:
1) Create new asp.net core app with (indivdual account) property.


2) Add the [`Google.Apis.Auth.AspNetCore3`](https://github.com/googleapis/google-api-dotnet-client) NuGet package to the app.


3) * Go to [Google API & Services](https://console.cloud.google.com/apis).
* A **Project** must exist first, you may have to create one. Once a project is selected, enter the **Dashboard**.

* In the **Oauth consent screen** of the **Dashboard**:
  * Select **User Type - External** and **CREATE**.
  * In the **App information** dialog, Provide an **app name** for the app, **user support email**, and **developer contact information**.
  * Step through the **Scopes** step.
  * Step through the **Test users** step.
  * Review the **OAuth consent screen** and go back to the app **Dashboard**.

* In the **Credentials** tab of the application Dashboard, select **CREATE CREDENTIALS** > **OAuth client ID**.
* Select **Application type** > **Web application**, choose a **name**.
* In the **Authorized redirect URIs** section, select **ADD URI** to set the redirect URI. Example redirect URI: `https://localhost:{PORT}/signin-google`, where the `{PORT}` placeholder is the app's port.
* Select the **CREATE** button.
* Save the **Client ID** and **Client Secret** for use in the app's configuration.
* When deploying the site, either:
  * Update the app's redirect URI in the **Google Console** to the app's deployed redirect URI.
  * Create a new Google API registration in the **Google Console** for the production app with its production redirect URI.

4) Store sensitive settings such as the Google client ID and secret values with [Secret Manager](xref:security/app-secrets). For this sample, use the following steps:

1. Initialize the project for secret storage per the instructions at [Enable secret storage](xref:security/app-secrets#enable-secret-storage).
1. Store the sensitive settings in the local secret store with the secret keys `Authentication:Google:ClientId` and `Authentication:Google:ClientSecret`:

    ```dotnetcli
    dotnet user-secrets set "Authentication:Google:ClientId" "<client-id>"
    dotnet user-secrets set "Authentication:Google:ClientSecret" "<client-secret>"
    ```
5) add auth part 

 ``` 
 builder.Services.AddAuthentication(o =>
{
      
}).AddCookie().AddGoogleOpenIdConnect(googleOptions =>
  {
      googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
      googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
  });
```
6) go to [google developer](https://developers.google.com/identity/gsi/web/guides/client-library) to get link of library and genrate your own code

7) setup your Controller to match with ``` data-login_uri``` attrbute.

8) your login function should take argument name ```string credential``` because that what google return when complete login process
```GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(credential);``` to validate your candiate if it valid you have now your credinatial you can added to the database