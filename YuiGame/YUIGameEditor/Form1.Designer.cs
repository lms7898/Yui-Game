namespace YUIGameEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ResetButton = new System.Windows.Forms.Button();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.SkillPoints = new System.Windows.Forms.TextBox();
            this.Health = new System.Windows.Forms.TextBox();
            this.Mana = new System.Windows.Forms.TextBox();
            this.MaxMana = new System.Windows.Forms.TextBox();
            this.MaxHealth = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Starting Health";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Starting Mana";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Skill Points";
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(124, 162);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(85, 69);
            this.ResetButton.TabIndex = 7;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(12, 162);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(84, 69);
            this.SubmitButton.TabIndex = 8;
            this.SubmitButton.Text = "Submit";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // SkillPoints
            // 
            this.SkillPoints.Location = new System.Drawing.Point(109, 113);
            this.SkillPoints.Name = "SkillPoints";
            this.SkillPoints.Size = new System.Drawing.Size(100, 20);
            this.SkillPoints.TabIndex = 22;
            this.SkillPoints.TextChanged += new System.EventHandler(this.SkillPoints_TextChanged);
            // 
            // Health
            // 
            this.Health.Location = new System.Drawing.Point(109, 9);
            this.Health.Name = "Health";
            this.Health.Size = new System.Drawing.Size(100, 20);
            this.Health.TabIndex = 23;
            this.Health.TextChanged += new System.EventHandler(this.Health_TextChanged);
            // 
            // Mana
            // 
            this.Mana.Location = new System.Drawing.Point(109, 35);
            this.Mana.Name = "Mana";
            this.Mana.Size = new System.Drawing.Size(100, 20);
            this.Mana.TabIndex = 24;
            this.Mana.TextChanged += new System.EventHandler(this.Mana_TextChanged);
            // 
            // MaxMana
            // 
            this.MaxMana.Location = new System.Drawing.Point(109, 87);
            this.MaxMana.Name = "MaxMana";
            this.MaxMana.Size = new System.Drawing.Size(100, 20);
            this.MaxMana.TabIndex = 30;
            this.MaxMana.TextChanged += new System.EventHandler(this.MaxMana_TextChanged);
            // 
            // MaxHealth
            // 
            this.MaxHealth.Location = new System.Drawing.Point(109, 61);
            this.MaxHealth.Name = "MaxHealth";
            this.MaxHealth.Size = new System.Drawing.Size(100, 20);
            this.MaxHealth.TabIndex = 29;
            this.MaxHealth.TextChanged += new System.EventHandler(this.MaxHealth_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 89);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Max Mana";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 63);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(61, 13);
            this.label14.TabIndex = 27;
            this.label14.Text = "Max Health";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 243);
            this.Controls.Add(this.MaxMana);
            this.Controls.Add(this.MaxHealth);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.Mana);
            this.Controls.Add(this.Health);
            this.Controls.Add(this.SkillPoints);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.TextBox SkillPoints;
        private System.Windows.Forms.TextBox Health;
        private System.Windows.Forms.TextBox Mana;
        private System.Windows.Forms.TextBox MaxMana;
        private System.Windows.Forms.TextBox MaxHealth;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
    }
}

