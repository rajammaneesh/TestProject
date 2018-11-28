using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DCode.Services.Common
{
    public class ODCReferenceService
    {
        /// <summary>
        /// Returns the ODCs present in the system 
        /// This reads the details from the Master ODC list, serializes and returns the list
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        public static ExistingODCList GetExistingODCList(string xmlFilePath)
        {
            ExistingODCList result;
            var xmlInputData = File.ReadAllText(xmlFilePath);
            XmlSerializer serializer = new XmlSerializer(typeof(ExistingODCList));
            using (StringReader reader = new StringReader(xmlInputData))
            {
                result = (ExistingODCList)(serializer.Deserialize(reader));
            }

            return result;
        }

        /// <summary>
        /// Get the ODC details by the offering ID
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="offeringId"></param>
        /// <returns></returns>
        public static ODC GetExistingODCByOfferingId(string xmlFilePath, string offeringId)
        {
            var odcList = GetExistingODCList(xmlFilePath);
            return odcList.ODCList.Where(off => off.OfferingId == offeringId).FirstOrDefault();
        }
    }

    /// <summary>
    /// Individual ODC node containing the details of an ODC
    /// </summary>
    [Serializable]
    public class ODC
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string OfferingId { get; set; }

        public string DistributionList { get; set; }

        public string ExcludeTaskTypeList { get; set; }

        public string Logo { get; set; }

        public string ShowTechX { get; set; }

        public string MailFooterText { get; set; }

        public string MailHeaderImage { get; set; }
    }

    /// <summary>
    /// Rootnode for serialization
    /// </summary>
    [Serializable, XmlRoot("ExistingODCList")]
    public class ExistingODCList
    {
        [XmlArray("ODCList")]
        [XmlArrayItem("ODC", typeof(ODC))]
        public ODC[] ODCList { get; set; }
    }
}