using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CustomPages.Configuration
{/// <summary>
    /// Usage example in .config file:
    /// <example>
    /// <code>
    ///     &lt;configuration>
    ///         &lt;configSections>
    ///             &lt;!--...-->
    ///             &lt;!--CustomPages Section declaration. (Just remove the , AssemblyName for web.config)-->
    ///             &lt;section name="CustomPagesParameters" type="CustomPages.Configuration.CustomPagesSection, CustomPages"/>
    ///             &lt;!--End of CustomPages Section declaration-->
    ///             &lt;!--...-->
    ///         &lt;/configSections>
    ///     &lt;/configuration>
    /// &lt;!--CustomPages Configuration Zone-->
    ///     &lt;CustomPagesParameters>
    ///         &lt;CustomPagesParameters>
    ///             &lt;add name="defaults" sitename="demo" mailgetter="demo@get.com" mailconfigsmtp="smtp.demo.com" mailconfigsmtpport="25" mailconfigsender="manager@demo.com" mailconfigpassword="demopwd" maildev="dev@demo.com"/>
    ///         &lt;/CustomPagesParameters>
    ///     &lt;/CustomPagesParameters>
    /// &lt;!--End Of CustomPages Configuration Zone-->    
    /// </code>
    /// </example>
    /// Mainly used to know the number of parameters line.
    /// Should throw an exception if >1. Will simply ignore other lines otherwise...
    /// </summary>
    public class CustomPagesSection : ConfigurationSection
    {
        internal const string ParameterName = "CustomPagesParameters";
        [ConfigurationProperty(ParameterName, IsDefaultCollection = true)]
        public CustomPagesCollection ParametersList
        {
            get { return (CustomPagesCollection)this[ParameterName]; }
            set { this[ParameterName] = value; }
        }
        /// <summary>
        /// Warning will only return the first line of parameters.
        /// if  WatermarkImageConfig.Configuration is used, that is already done.
        /// Multi line is not supported. Please check for multiline with ParametersList before in case of self overriding of the code.
        /// </summary>
        public CustomPagesElement Parameters
        {
            get
            { return ParametersList.Cast<CustomPagesElement>().FirstOrDefault(); }
            //set { this[ParameterName] = value; }
        }
    }
}
