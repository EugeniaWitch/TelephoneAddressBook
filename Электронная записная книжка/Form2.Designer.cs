using System.Windows.Forms;

namespace Электронная_записная_книжка
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.richTextBoxDisplayName = new System.Windows.Forms.RichTextBox();
            this.richTextBoxCompany = new System.Windows.Forms.RichTextBox();
            this.richTextBoxPosition = new System.Windows.Forms.RichTextBox();
            this.richTextBoxAddress = new System.Windows.Forms.RichTextBox();
            this.richTextBoxPhone1 = new System.Windows.Forms.RichTextBox();
            this.richTextBoxPhone2 = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.richTextBoxPhone3 = new System.Windows.Forms.RichTextBox();
            this.richTextBoxEmail1 = new System.Windows.Forms.RichTextBox();
            this.richTextBoxEmail2 = new System.Windows.Forms.RichTextBox();
            this.richTextBoxNotes = new System.Windows.Forms.RichTextBox();
            this.Redact = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.dateTimePickerBirthday = new System.Windows.Forms.DateTimePicker();
            this.labelImportant = new System.Windows.Forms.Label();
            this.richTextBoxGroups = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(265, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(250, 250);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(67, 292);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "ФИО:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(11, 492);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Номер телефона:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(23, 332);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "День рождения:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(58, 372);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 21);
            this.label4.TabIndex = 4;
            this.label4.Text = "Работа:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(335, 456);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(190, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "Электронная почта:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(62, 452);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 21);
            this.label6.TabIndex = 6;
            this.label6.Text = "Адрес:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(14, 692);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(163, 21);
            this.label7.TabIndex = 7;
            this.label7.Text = "Доп. информация:";
            // 
            // richTextBoxDisplayName
            // 
            this.richTextBoxDisplayName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBoxDisplayName.Location = new System.Drawing.Point(190, 290);
            this.richTextBoxDisplayName.Name = "richTextBoxDisplayName";
            this.richTextBoxDisplayName.ReadOnly = true;
            this.richTextBoxDisplayName.Size = new System.Drawing.Size(500, 30);
            this.richTextBoxDisplayName.TabIndex = 8;
            this.richTextBoxDisplayName.Text = "";
            // 
            // richTextBoxCompany
            // 
            this.richTextBoxCompany.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBoxCompany.Location = new System.Drawing.Point(190, 370);
            this.richTextBoxCompany.Name = "richTextBoxCompany";
            this.richTextBoxCompany.ReadOnly = true;
            this.richTextBoxCompany.Size = new System.Drawing.Size(500, 30);
            this.richTextBoxCompany.TabIndex = 10;
            this.richTextBoxCompany.Text = "";
            // 
            // richTextBoxPosition
            // 
            this.richTextBoxPosition.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBoxPosition.Location = new System.Drawing.Point(190, 410);
            this.richTextBoxPosition.Name = "richTextBoxPosition";
            this.richTextBoxPosition.ReadOnly = true;
            this.richTextBoxPosition.Size = new System.Drawing.Size(500, 30);
            this.richTextBoxPosition.TabIndex = 11;
            this.richTextBoxPosition.Text = "";
            // 
            // richTextBoxAddress
            // 
            this.richTextBoxAddress.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBoxAddress.Location = new System.Drawing.Point(190, 450);
            this.richTextBoxAddress.Name = "richTextBoxAddress";
            this.richTextBoxAddress.ReadOnly = true;
            this.richTextBoxAddress.Size = new System.Drawing.Size(500, 30);
            this.richTextBoxAddress.TabIndex = 12;
            this.richTextBoxAddress.Text = "";
            // 
            // richTextBoxPhone1
            // 
            this.richTextBoxPhone1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBoxPhone1.Location = new System.Drawing.Point(190, 490);
            this.richTextBoxPhone1.Name = "richTextBoxPhone1";
            this.richTextBoxPhone1.ReadOnly = true;
            this.richTextBoxPhone1.Size = new System.Drawing.Size(500, 30);
            this.richTextBoxPhone1.TabIndex = 13;
            this.richTextBoxPhone1.Text = "";
            // 
            // richTextBoxPhone2
            // 
            this.richTextBoxPhone2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBoxPhone2.Location = new System.Drawing.Point(190, 530);
            this.richTextBoxPhone2.Name = "richTextBoxPhone2";
            this.richTextBoxPhone2.ReadOnly = true;
            this.richTextBoxPhone2.Size = new System.Drawing.Size(500, 30);
            this.richTextBoxPhone2.TabIndex = 14;
            this.richTextBoxPhone2.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(48, 612);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 21);
            this.label8.TabIndex = 15;
            this.label8.Text = "Эл. почта:";
            // 
            // richTextBoxPhone3
            // 
            this.richTextBoxPhone3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBoxPhone3.Location = new System.Drawing.Point(190, 570);
            this.richTextBoxPhone3.Name = "richTextBoxPhone3";
            this.richTextBoxPhone3.ReadOnly = true;
            this.richTextBoxPhone3.Size = new System.Drawing.Size(500, 30);
            this.richTextBoxPhone3.TabIndex = 16;
            this.richTextBoxPhone3.Text = "";
            // 
            // richTextBoxEmail1
            // 
            this.richTextBoxEmail1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBoxEmail1.Location = new System.Drawing.Point(190, 610);
            this.richTextBoxEmail1.Name = "richTextBoxEmail1";
            this.richTextBoxEmail1.ReadOnly = true;
            this.richTextBoxEmail1.Size = new System.Drawing.Size(500, 30);
            this.richTextBoxEmail1.TabIndex = 17;
            this.richTextBoxEmail1.Text = "";
            // 
            // richTextBoxEmail2
            // 
            this.richTextBoxEmail2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBoxEmail2.Location = new System.Drawing.Point(190, 650);
            this.richTextBoxEmail2.Name = "richTextBoxEmail2";
            this.richTextBoxEmail2.ReadOnly = true;
            this.richTextBoxEmail2.Size = new System.Drawing.Size(500, 30);
            this.richTextBoxEmail2.TabIndex = 18;
            this.richTextBoxEmail2.Text = "";
            // 
            // richTextBoxNotes
            // 
            this.richTextBoxNotes.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBoxNotes.Location = new System.Drawing.Point(190, 690);
            this.richTextBoxNotes.Name = "richTextBoxNotes";
            this.richTextBoxNotes.ReadOnly = true;
            this.richTextBoxNotes.Size = new System.Drawing.Size(500, 151);
            this.richTextBoxNotes.TabIndex = 19;
            this.richTextBoxNotes.Text = "";
            // 
            // Redact
            // 
            this.Redact.BackColor = System.Drawing.Color.LightGreen;
            this.Redact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Redact.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Redact.ForeColor = System.Drawing.Color.ForestGreen;
            this.Redact.Location = new System.Drawing.Point(92, 877);
            this.Redact.Name = "Redact";
            this.Redact.Size = new System.Drawing.Size(200, 50);
            this.Redact.TabIndex = 20;
            this.Redact.Text = "Изменить";
            this.Redact.UseVisualStyleBackColor = false;
            this.Redact.Click += new System.EventHandler(this.Redact_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.MistyRose;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Tomato;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ForeColor = System.Drawing.Color.Tomato;
            this.button2.Location = new System.Drawing.Point(500, 879);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(200, 50);
            this.button2.TabIndex = 21;
            this.button2.Text = "Удалить";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.Delete_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(40, 412);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 21);
            this.label9.TabIndex = 22;
            this.label9.Text = "Профессия:";
            // 
            // dateTimePickerBirthday
            // 
            this.dateTimePickerBirthday.Location = new System.Drawing.Point(190, 330);
            this.dateTimePickerBirthday.Name = "dateTimePickerBirthday";
            this.dateTimePickerBirthday.Size = new System.Drawing.Size(500, 26);
            this.dateTimePickerBirthday.TabIndex = 23;
            // 
            // labelImportant
            // 
            this.labelImportant.AutoSize = true;
            this.labelImportant.Font = new System.Drawing.Font("Arial", 30F);
            this.labelImportant.ForeColor = System.Drawing.Color.Gold;
            this.labelImportant.Location = new System.Drawing.Point(190, 218);
            this.labelImportant.Name = "labelImportant";
            this.labelImportant.Size = new System.Drawing.Size(0, 67);
            this.labelImportant.TabIndex = 24;
            // 
            // richTextBoxGroups
            // 
            this.richTextBoxGroups.BackColor = System.Drawing.Color.Gold;
            this.richTextBoxGroups.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxGroups.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxGroups.Location = new System.Drawing.Point(533, 251);
            this.richTextBoxGroups.Name = "richTextBoxGroups";
            this.richTextBoxGroups.ReadOnly = true;
            this.richTextBoxGroups.Size = new System.Drawing.Size(157, 30);
            this.richTextBoxGroups.TabIndex = 25;
            this.richTextBoxGroups.Text = "";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 944);
            this.Controls.Add(this.richTextBoxGroups);
            this.Controls.Add(this.labelImportant);
            this.Controls.Add(this.dateTimePickerBirthday);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Redact);
            this.Controls.Add(this.richTextBoxNotes);
            this.Controls.Add(this.richTextBoxEmail2);
            this.Controls.Add(this.richTextBoxEmail1);
            this.Controls.Add(this.richTextBoxPhone3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.richTextBoxPhone2);
            this.Controls.Add(this.richTextBoxPhone1);
            this.Controls.Add(this.richTextBoxAddress);
            this.Controls.Add(this.richTextBoxPosition);
            this.Controls.Add(this.richTextBoxCompany);
            this.Controls.Add(this.richTextBoxDisplayName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Окно контакта";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox richTextBoxDisplayName;
        private System.Windows.Forms.RichTextBox richTextBoxCompany;
        private System.Windows.Forms.RichTextBox richTextBoxPosition;
        private System.Windows.Forms.RichTextBox richTextBoxAddress;
        private System.Windows.Forms.RichTextBox richTextBoxPhone1;
        private System.Windows.Forms.RichTextBox richTextBoxPhone2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox richTextBoxPhone3;
        private System.Windows.Forms.RichTextBox richTextBoxEmail1;
        private System.Windows.Forms.RichTextBox richTextBoxEmail2;
        private System.Windows.Forms.RichTextBox richTextBoxNotes;
        private System.Windows.Forms.Button Redact;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dateTimePickerBirthday;
        private System.Windows.Forms.Label labelImportant;
        private System.Windows.Forms.RichTextBox richTextBoxGroups;
    }
}