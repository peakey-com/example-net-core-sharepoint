# Simple sample app using an app to publish current temperature to a sharepoint list
Once everything below is configured, you should able to run this app from console and it will add the current temperature (Warsaw, IN) to your sharepoint list with a timestamp.

In order to run this you will need.
 * a sharepoint site
   * Create a list
     * In addition to the "Title" column, add a new single line text column "Temperature"
 
 * azure active diretory
   * register an app in "App Registrations"
     * (Permissions) SharePoint
       * Sites.Read.All
       * Sites.ReadWrite.All
    * Create and Upload Certificate (cer)
 
* Edit "Program.cs"
  * Set
    * SHAREPOINT_DOMAIN
     * APPLICATION_CLIENT_ID
     * DIRECTORY_TENANT_ID
     * CERTIFICATE_PATH (pfx)
     * CERTIFICATE_PASSWORD
     * LIST_NAME