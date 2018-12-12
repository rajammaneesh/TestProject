using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DCode.Models.ODC
{
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
