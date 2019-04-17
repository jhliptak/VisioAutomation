﻿using SMA = System.Management.Automation;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands.VisioDocument
{
    [SMA.Cmdlet(SMA.VerbsCommon.Close, Nouns.VisioDocument)]
    public class CloseVisioDocument : VisioCmdlet
    {
        [SMA.Parameter(Mandatory = false)]
        public IVisio.Document[] Document;

        [SMA.Parameter(Mandatory = false)]
        public SMA.SwitchParameter Force;

        protected override void ProcessRecord()
        {
            var targetdocs = new VisioScripting.TargetDocuments(this.Document);
            this.Client.Document.CloseDocuments(targetdocs, this.Force);
        }
    }
}