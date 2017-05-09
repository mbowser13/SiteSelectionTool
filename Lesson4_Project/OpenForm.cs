using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Lesson4_Project
{
    public class OpenForm : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public OpenForm()
        {
        }

        protected override void OnClick()
        {
            Lesson4_Form myForm = new Lesson4_Form();
            myForm.ShowDialog();

            ArcMap.Application.CurrentTool = null;
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
