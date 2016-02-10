using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts
{
    public static class CertificateHelper
    {
        public class IgnoreCertificateErrorsValidator : X509CertificateValidator
        {
            public IgnoreCertificateErrorsValidator()
                : base()
            {

            }
            public override void LoadCustomConfiguration(System.Xml.XmlNodeList nodelist)
            {
                base.LoadCustomConfiguration(nodelist);
            }

            public override void Validate(System.Security.Cryptography.X509Certificates.X509Certificate2 certificate)
            {
                // We avoid validating the certificate 
            }
        }

        public static X509Certificate2 GetCertificate(StoreName name, StoreLocation location, X509FindType findType, object findValue)
        {
            X509Store store = new X509Store(name, location);
            X509Certificate2Collection found = null;
            store.Open(OpenFlags.ReadOnly);

            try
            {
                found = store.Certificates.Find(findType, findValue, true);

                if (found.Count == 0)
                    throw new ArgumentException(string.Format("No certificate was found matching the specified criteria."));


                if (found.Count > 1 && findType == X509FindType.FindBySubjectName)
                {
                    X509Certificate2Collection foundBuffer = new X509Certificate2Collection();
                    foreach (var item in found)
                    {
                        if (item.Subject == "CN=" + findValue)
                        {
                            foundBuffer.Add(item);
                        }
                    }
                    found = foundBuffer;
                }

                if (found.Count > 1)
                    throw new ArgumentException(string.Format("There are more than one certificate matching the specified criteria."));

                return new X509Certificate2(found[0]);
            }
            finally
            {
                if (found != null)
                {
                    foreach (X509Certificate2 cert in found)
                    {
                        cert.Reset();
                    }
                }

                store.Close();
            }
        }

    }
}
