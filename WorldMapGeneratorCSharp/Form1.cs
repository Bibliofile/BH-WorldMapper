using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WorldMapGeneratorCSharp
{
	public partial class Form1 : Form
	{
		int worldNormal = 16384;
		int world4x = 65536;
		int world16x = 262144;
		Bitmap picture;
		Boolean canGenerate;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			btnGenerate.Enabled = false;
			btnSave.Enabled = false;
			canGenerate = false;
		}

		private void btnFolderPick_Click(object sender, EventArgs e)
		{
			DialogResult result;
			progressBar1.Value = 0;
			result = folderBrowserDialog1.ShowDialog();
			if (canGenerate && result == DialogResult.OK) 
			{
				canGenerate = false;
				btnGenerate.Enabled = true;
			} 
			else if (result == DialogResult.OK)
			{
				canGenerate = true;
			}
		}

		//Radio Buttons
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			if (canGenerate)
			{
				btnGenerate.Enabled = true;
			}
			else if (radioButton1.Checked)
			{
				canGenerate = true;
			}
		}
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			if (canGenerate)
			{
				btnGenerate.Enabled = true;
			}
			else if (radioButton2.Checked)
			{
				canGenerate = true;
			}
		}
		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			if (canGenerate)
			{
				btnGenerate.Enabled = true;
			}
			else if (radioButton3.Checked)
			{
				canGenerate = true;
			}
		}

		//Here is where all generation is done
		private void btnGenerate_Click(object sender, EventArgs e)
		{
			string location = folderBrowserDialog1.SelectedPath;
			int size;
			RadioButton modeRadio;

			//Get the selected world size
			modeRadio = groupBox1.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked);
			if (modeRadio.Name == "radioButton1")
			{
				size = worldNormal;
			}
			else if (modeRadio.Name == "radioButton2")
			{
				size = world4x;
			}
			else if (modeRadio.Name == "radioButton3")
			{
				size = world16x;
			}
			else
			{
				MessageBox.Show("Size error, please select an allowed size");
				return;
			}

			string[] fileArray = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
			Bitmap result = new Bitmap(size, 1024, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			progressBar1.Maximum = fileArray.Length;

			using (Graphics g = Graphics.FromImage(result))
			{
				foreach (string picLoc in fileArray)
				{
					string[] seperators = { "\\", "_", "." };
					string[] picSplit = picLoc.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
					string xCoord = picSplit[picSplit.Length - 3];
					string yCoord = picSplit[picSplit.Length - 2];

					using (Bitmap layer = new Bitmap(Image.FromFile(picLoc)))
					{
						g.DrawImage(layer, (int.Parse(xCoord) * 32), (1024 - int.Parse(yCoord) * 32), 32, 32);
					}
					progressBar1.Increment(1);
				}
			}
			picture = result;
			btnSave.Enabled = true;
		}

		//Save the image as BMP
		private void btnSave_Click(object sender, EventArgs e)
		{
			saveFileDialog1.ShowDialog();
		}
		private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
		{
			picture.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
		}

	}
}
