using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using BusinessLayerCodeGenerator;
using BusinessLayerCodeGenerator.Enums;
using GeneratorBusiness;
using GeneratorBusiness.DTOs;
using PropertyInfo = GeneratorBusiness.DTOs.PropertyInfo;

namespace ADO_CodeGenerator
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        void RefreshTextScreen()
        {
            //  txtScreen.Text = Template.ToString();   
        }


        private void txtPath_Validating(object sender, CancelEventArgs e)
        {
            //TextBox PathTextBox= (TextBox)sender;
            //if (string.IsNullOrWhiteSpace(PathTextBox.Text))
            //{
            //    errorProvider1.SetError(PathTextBox, "Path cannot be empty!");
            //    //e.Cancel = true;    
            //}
            //else
            //{
            //    errorProvider1.SetError(PathTextBox, "");
            //    e.Cancel = false;
            //}
        }






        //void AppendPropertyName()
        //{
        //    _Template.AppendLine($"    {cbAccessSpecifier.Text} {cbDataType.Text} {txtPropertyName.Text} {PropertyDefinitionBasedOnSelectedAccessor()}");
        //    RefreshTextScreen();
        //}
        private void txtDataAccessPath_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDataAcessPath.Text))
            {
                errorProvider1.SetError(txtDataAcessPath, "Path cannot be empty!");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtDataAcessPath, "");
                e.Cancel = false;


            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rbDotNetCore.Checked = true;
        }





        string ExtractProjectNameFromPath()
        {
            string[] Names = txtBusinessPath.Text.Split('\\');
            return Names[Names.Length - 1];
        }

        void GenerateCode()
        {
            StringBuilder Template = new StringBuilder();
            Template.AppendLine("using System;\n");
            Template.Append($"namespace {ExtractProjectNameFromPath()}");

        }

        bool ValidatePaths()
        {
            if (txtDataAcessPath.Text == "" || txtBusinessPath.Text == "")
            {
                return false;
            }

            return true;
        }
        enum enProjectLayer { Business, DataAccess }





        private void btnGenerateClass_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                return;
            }
            if (!ValidatePaths())
            {
                MessageBox.Show("Enter valid Paths please", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Start Generating?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            var BusinessDTO = new BusinessGeneratorDTO();
            BusinessDTO.ProjectPath = txtBusinessPath.Text;
            BusinessDTO.FolderName = txtBusinessFolder.Text;


            BusinessDTO.PropertiesInfo = new List<PropertyInfo>();


            //new GeneratorBusiness.GenerateBusiness(BusinessDTO).Generate();

            var DataAccessDTO = new DataAccessGeneratorDTO()
            {
                FolderName = txtDataAccessFolder.Text,
                DataAccessProjectPath = txtDataAcessPath.Text,
                TargetPlatform = rbDotNetCore.Checked ? enTargetPlatform.NetCore : enTargetPlatform.NetFramework,
                PresentationProjectPath = txtPresentationPath.Text

            };
            new GenerateDataAccess(DataAccessDTO).Generate();




        }



        private void btnAddPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (sender == btnAddBusinessPath)
                {
                    txtBusinessPath.Text = folderBrowserDialog1.SelectedPath;
                }
                else if (sender == btnAddDataAccessPath)
                {
                    txtDataAcessPath.Text = folderBrowserDialog1.SelectedPath;
                }
                else
                {
                    txtPresentationPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

      

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((TextBox)cmsProjectPathTextBox.SourceControl == txtBusinessPath)
            {
                txtBusinessPath.Clear();
            }
            else if ((TextBox)cmsProjectPathTextBox.SourceControl == txtBusinessPath)
            {
                txtDataAcessPath.Clear();
            }
        }


    }
}

