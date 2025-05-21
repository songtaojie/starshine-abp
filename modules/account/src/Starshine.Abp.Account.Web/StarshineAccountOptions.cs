namespace Starshine.Abp.Account.Web;

public class StarshineAccountOptions
{
    /// <summary>
    /// Default value: "Windows".
    /// </summary>
    public string WindowsAuthenticationSchemeName { get; set; }

    public StarshineAccountOptions()
    {
        //TODO: This makes us depend on the Microsoft.AspNetCore.Server.IISIntegration package.
        WindowsAuthenticationSchemeName = "Windows"; //Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
    }
}
