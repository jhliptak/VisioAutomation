using IVisio=Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;
using SMA = System.Management.Automation;

namespace VisioPS.Commands
{
    [SMA.Cmdlet(SMA.VerbsCommon.Get, "VisioMaster")]
    public class Get_VisioMaster : VisioPSCmdlet
    {
        [SMA.Parameter(Position = 0, Mandatory = false)]
        public string Name;

        [SMA.Parameter(Position = 1, Mandatory = false, ParameterSetName = "StencilDoc")]
        public IVisio.Document Document;

        protected override void ProcessRecord()
        {
            var scriptingsession = this.ScriptingSession;

            bool master_specified = this.Name !=null;
            bool doc_specified = this.Document !=null;

            if (master_specified)
            {
                // master name is specified
                if (doc_specified)
                {
                    this.WriteVerbose("Get master from specified document");
                    var masters = scriptingsession.Master.GetMastersByName(this.Name, this.Document);
                    this.WriteObject(masters,true);
                }
                else
                {
                    this.WriteVerbose("Get master from active document");
                    var masters = scriptingsession.Master.GetMastersByName(this.Name);
                    this.WriteObject(masters,true);
                }
            }
            else
            {
                // master name is not specified
                if (doc_specified)
                {
                    this.WriteVerbose("Get all masters from specified document");
                    var masters = scriptingsession.Master.Get(this.Document);
                    this.WriteObject(masters, false);                    
                }
                else
                {
                    this.WriteVerbose("Get all masters from active document");
                    var masters = scriptingsession.Master.Get();
                    this.WriteObject(masters, false);                   
                }
            }
        }
    }
}