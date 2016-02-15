using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Sts
{
    public static class CertificateHelper
    {

        public static X509Certificate2 GetCertificate(StoreName name, StoreLocation location, X509FindType findType, object findValue)
        {
            X509Store store = new X509Store(name, location);
            X509Certificate2Collection found = null;
            store.Open(OpenFlags.ReadOnly);

            try
            {
                found = store.Certificates.Find(findType, findValue, true);

                if (found.Count == 0)
                    throw new Exception($"No certificate was found matching the specified criteria. Type: {findType} Value: {findValue}");


                if (found.Count > 1 && findType == X509FindType.FindBySubjectName)
                {
                    X509Certificate2Collection foundBuffer = new X509Certificate2Collection();
                    foreach (var item in found)
                    {
                        if (item.Subject == $"CN={findValue}" )
                        {
                            foundBuffer.Add(item);
                        }
                    }
                    found = foundBuffer;
                }

                if (found.Count > 1)
                    throw new ArgumentException("There are more than one certificate matching the specified criteria.");

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

        public static X509Certificate2 GetCertificate(this CertificateReferenceElement reference)
        {
            if (reference != null)
            {
                return GetCertificate(
                    reference.StoreName,
                    reference.StoreLocation,
                    reference.X509FindType,
                    reference.FindValue);
            }

            return null;
        }

        /// <summary>
        /// Sign data using a certificate. Warning: the certificate must be reloaded!
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SignData(X509Certificate2 cert, string data)
        {
            using (var csp = (RSACryptoServiceProvider)cert.PrivateKey)
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    var toSign = Encoding.UTF8.GetBytes(data);
                    var dataHash = sha1.ComputeHash(toSign);
                    return Convert.ToBase64String(csp.SignHash(dataHash, CryptoConfig.MapNameToOID("SHA1")));
                }
            }
        }

        /// <summary>
        /// Verify sign data using a certificate. Warning: the certificate must be reloaded!
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static bool VerifySingedData(X509Certificate2 cert, string data, string signature)
        {
            var singatureData = Convert.FromBase64String(signature);
            using (RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key)
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    var toSign = Encoding.UTF8.GetBytes(data);
                    var dataHash = sha1.ComputeHash(toSign);
                    return csp.VerifyHash(dataHash, CryptoConfig.MapNameToOID("SHA1"), singatureData);
                }
            }
        }


    }
}
