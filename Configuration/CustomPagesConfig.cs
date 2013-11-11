using System;
using System.Configuration;

namespace CustomPages.Configuration
{
    public class CustomPagesConfig
    {
        public static CustomPagesSection Configuration
        {
            get
            {
                CustomPagesSection wis = null;
                try
                {
                    wis = ConfigurationManager.GetSection(CustomPagesSection.ParameterName) as CustomPagesSection;
                    if(wis!=null&&wis.ParametersList.Count>1)throw new ConfigurationErrorsException("Error there are more than one (1) line of parameters.");
                }
                catch (ConfigurationErrorsException cee)
                {
                    Console.WriteLine(cee.BareMessage);
                    //no Web.config section detected, will use default parameters OR let developper set them later...
                }
                return wis;
            }
        }
    }
}
