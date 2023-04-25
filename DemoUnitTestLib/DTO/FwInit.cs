
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using LabCommonLib;

namespace DemoUnitTestLib.DTO
{
    [DataContract(), Table("FW_INIT")]
    public partial class FwInit : BaseDto
    {


        [DataMember(), DataField("ACCESSDATE")]
        public DateTime? Accessdate { get; set; }
        [DataMember(), DataField("CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }
        [DataMember(), DataField("CREATED_USER")]
        public String CreatedUser { get; set; }
        [DataMember(), DataField("DESCR")]
        public String Descr { get; set; }
        [DataMember(), DataField("FUNCTION")]
        public String Function { get; set; }
        [DataMember(), DataField("KEY_NAME")]
        public String KeyName { get; set; }
        [DataMember(), DataField("OWNER")]
        public String Owner { get; set; }
        [DataMember(), DataField("PROGRAMCODE")]
        public String Programcode { get; set; }
        [DataMember(), DataField("PROGRAM_ID")]
        public String ProgramId { get; set; }
        [DataMember(), DataField("VALUE")]
        public String Value { get; set; }
    }
}
