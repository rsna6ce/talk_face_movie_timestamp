// Form1.Designer.cs
namespace TalkFaceMovieTimestamp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnMark = new System.Windows.Forms.Button();
            this.btnLoadCsv = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblWaveFile = new System.Windows.Forms.Label();
            this.lstTimestamps = new System.Windows.Forms.ListBox();
            this.lblTimer = new System.Windows.Forms.Label();
            this.btnFromMinus = new System.Windows.Forms.Button();
            this.btnFromPlus = new System.Windows.Forms.Button();
            this.btnToMinus = new System.Windows.Forms.Button();
            this.btnToPlus = new System.Windows.Forms.Button();
            this.lblCsvFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 8);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 27);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "WAV選択";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 72);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(244, 27);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.Text = "再生/停止";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(144, 156);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(111, 27);
            this.btnVerify.TabIndex = 0;
            this.btnVerify.Text = "検証再生/停止";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.BtnVerify_Click);
            // 
            // btnMark
            // 
            this.btnMark.Location = new System.Drawing.Point(12, 104);
            this.btnMark.Name = "btnMark";
            this.btnMark.Size = new System.Drawing.Size(244, 44);
            this.btnMark.TabIndex = 0;
            this.btnMark.Text = "タイムスタンプマーク 開始/終了\r\n (長押し)";
            this.btnMark.UseVisualStyleBackColor = true;
            this.btnMark.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnMark_MouseDown);
            this.btnMark.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnMark_MouseUp);
            // 
            // btnLoadCsv
            // 
            this.btnLoadCsv.Location = new System.Drawing.Point(12, 40);
            this.btnLoadCsv.Name = "btnLoadCsv";
            this.btnLoadCsv.Size = new System.Drawing.Size(75, 27);
            this.btnLoadCsv.TabIndex = 0;
            this.btnLoadCsv.Text = "CSV読込";
            this.btnLoadCsv.UseVisualStyleBackColor = true;
            this.btnLoadCsv.Click += new System.EventHandler(this.BtnLoadCsv_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 384);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(244, 27);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "CSV保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // lblWaveFile
            // 
            this.lblWaveFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWaveFile.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaveFile.Location = new System.Drawing.Point(92, 12);
            this.lblWaveFile.Name = "lblWaveFile";
            this.lblWaveFile.Size = new System.Drawing.Size(160, 20);
            this.lblWaveFile.TabIndex = 1;
            this.lblWaveFile.Text = "              ";
            // 
            // lstTimestamps
            // 
            this.lstTimestamps.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstTimestamps.FormattingEnabled = true;
            this.lstTimestamps.HorizontalScrollbar = true;
            this.lstTimestamps.Location = new System.Drawing.Point(16, 192);
            this.lstTimestamps.Name = "lstTimestamps";
            this.lstTimestamps.Size = new System.Drawing.Size(240, 147);
            this.lstTimestamps.TabIndex = 2;
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimer.Location = new System.Drawing.Point(16, 160);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(90, 19);
            this.lblTimer.TabIndex = 3;
            this.lblTimer.Text = "00:00.000";
            // 
            // btnFromMinus
            // 
            this.btnFromMinus.Location = new System.Drawing.Point(16, 348);
            this.btnFromMinus.Name = "btnFromMinus";
            this.btnFromMinus.Size = new System.Drawing.Size(56, 27);
            this.btnFromMinus.TabIndex = 0;
            this.btnFromMinus.Text = "from <";
            this.btnFromMinus.UseVisualStyleBackColor = true;
            // 
            // btnFromPlus
            // 
            this.btnFromPlus.Location = new System.Drawing.Point(76, 348);
            this.btnFromPlus.Name = "btnFromPlus";
            this.btnFromPlus.Size = new System.Drawing.Size(56, 27);
            this.btnFromPlus.TabIndex = 0;
            this.btnFromPlus.Text = "from >";
            this.btnFromPlus.UseVisualStyleBackColor = true;
            // 
            // btnToMinus
            // 
            this.btnToMinus.Location = new System.Drawing.Point(144, 348);
            this.btnToMinus.Name = "btnToMinus";
            this.btnToMinus.Size = new System.Drawing.Size(56, 27);
            this.btnToMinus.TabIndex = 0;
            this.btnToMinus.Text = "to <";
            this.btnToMinus.UseVisualStyleBackColor = true;
            // 
            // btnToPlus
            // 
            this.btnToPlus.Location = new System.Drawing.Point(204, 348);
            this.btnToPlus.Name = "btnToPlus";
            this.btnToPlus.Size = new System.Drawing.Size(56, 27);
            this.btnToPlus.TabIndex = 0;
            this.btnToPlus.Text = "to >";
            this.btnToPlus.UseVisualStyleBackColor = true;
            // 
            // lblCsvFile
            // 
            this.lblCsvFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCsvFile.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCsvFile.Location = new System.Drawing.Point(92, 44);
            this.lblCsvFile.Name = "lblCsvFile";
            this.lblCsvFile.Size = new System.Drawing.Size(160, 20);
            this.lblCsvFile.TabIndex = 1;
            this.lblCsvFile.Text = "              ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 425);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.lstTimestamps);
            this.Controls.Add(this.lblCsvFile);
            this.Controls.Add(this.lblWaveFile);
            this.Controls.Add(this.btnMark);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnToPlus);
            this.Controls.Add(this.btnToMinus);
            this.Controls.Add(this.btnFromPlus);
            this.Controls.Add(this.btnFromMinus);
            this.Controls.Add(this.btnLoadCsv);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnLoad);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "talk_face_movie_timestamp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnMark;
        private System.Windows.Forms.Button btnLoadCsv;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblWaveFile;
        private System.Windows.Forms.ListBox lstTimestamps;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Button btnFromMinus;
        private System.Windows.Forms.Button btnFromPlus;
        private System.Windows.Forms.Button btnToMinus;
        private System.Windows.Forms.Button btnToPlus;
        private System.Windows.Forms.Label lblCsvFile;
    }
}