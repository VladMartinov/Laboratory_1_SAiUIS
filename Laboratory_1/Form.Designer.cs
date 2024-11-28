namespace Laboratory_1
{
    partial class Form
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonSelectData = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.listBoxDefault = new System.Windows.Forms.ListBox();
            this.listBoxKolmogorovGabor = new System.Windows.Forms.ListBox();
            this.groupBoxActions = new System.Windows.Forms.GroupBox();
            this.buttonPredict = new System.Windows.Forms.Button();
            this.buttonSelectTarget = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.listBoxDefaultPredict = new System.Windows.Forms.ListBox();
            this.listBoxKolmogorovGaborPredict = new System.Windows.Forms.ListBox();
            this.groupBoxActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Excel Files (*.xls, *.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            // 
            // buttonSelectData
            // 
            this.buttonSelectData.Location = new System.Drawing.Point(6, 19);
            this.buttonSelectData.Name = "buttonSelectData";
            this.buttonSelectData.Size = new System.Drawing.Size(89, 23);
            this.buttonSelectData.TabIndex = 0;
            this.buttonSelectData.Text = "Выбор данных";
            this.buttonSelectData.UseVisualStyleBackColor = true;
            this.buttonSelectData.Click += new System.EventHandler(this.ButtonSelectData_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(6, 91);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(89, 36);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Старт обучения";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // listBoxDefault
            // 
            this.listBoxDefault.FormattingEnabled = true;
            this.listBoxDefault.Location = new System.Drawing.Point(12, 12);
            this.listBoxDefault.Name = "listBoxDefault";
            this.listBoxDefault.Size = new System.Drawing.Size(638, 199);
            this.listBoxDefault.TabIndex = 3;
            // 
            // listBoxKolmogorovGabor
            // 
            this.listBoxKolmogorovGabor.FormattingEnabled = true;
            this.listBoxKolmogorovGabor.Location = new System.Drawing.Point(12, 239);
            this.listBoxKolmogorovGabor.Name = "listBoxKolmogorovGabor";
            this.listBoxKolmogorovGabor.Size = new System.Drawing.Size(638, 199);
            this.listBoxKolmogorovGabor.TabIndex = 4;
            // 
            // groupBoxActions
            // 
            this.groupBoxActions.Controls.Add(this.buttonPredict);
            this.groupBoxActions.Controls.Add(this.buttonSelectTarget);
            this.groupBoxActions.Controls.Add(this.buttonClear);
            this.groupBoxActions.Controls.Add(this.buttonSelectData);
            this.groupBoxActions.Controls.Add(this.buttonStart);
            this.groupBoxActions.Location = new System.Drawing.Point(656, 12);
            this.groupBoxActions.Name = "groupBoxActions";
            this.groupBoxActions.Size = new System.Drawing.Size(101, 426);
            this.groupBoxActions.TabIndex = 5;
            this.groupBoxActions.TabStop = false;
            this.groupBoxActions.Text = "Действия";
            // 
            // buttonPredict
            // 
            this.buttonPredict.Location = new System.Drawing.Point(6, 133);
            this.buttonPredict.Name = "buttonPredict";
            this.buttonPredict.Size = new System.Drawing.Size(89, 23);
            this.buttonPredict.TabIndex = 5;
            this.buttonPredict.Text = "Предсказание";
            this.buttonPredict.UseVisualStyleBackColor = true;
            this.buttonPredict.Click += new System.EventHandler(this.ButtonPredict_Click);
            // 
            // buttonSelectTarget
            // 
            this.buttonSelectTarget.Location = new System.Drawing.Point(6, 48);
            this.buttonSelectTarget.Name = "buttonSelectTarget";
            this.buttonSelectTarget.Size = new System.Drawing.Size(89, 37);
            this.buttonSelectTarget.TabIndex = 4;
            this.buttonSelectTarget.Text = "Выбор целевых данных";
            this.buttonSelectTarget.UseVisualStyleBackColor = true;
            this.buttonSelectTarget.Click += new System.EventHandler(this.ButtonSelectTarget_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(6, 384);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(89, 36);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Очистка";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // listBoxDefaultPredict
            // 
            this.listBoxDefaultPredict.FormattingEnabled = true;
            this.listBoxDefaultPredict.Location = new System.Drawing.Point(763, 12);
            this.listBoxDefaultPredict.Name = "listBoxDefaultPredict";
            this.listBoxDefaultPredict.Size = new System.Drawing.Size(406, 199);
            this.listBoxDefaultPredict.TabIndex = 6;
            // 
            // listBoxKolmogorovGaborPredict
            // 
            this.listBoxKolmogorovGaborPredict.FormattingEnabled = true;
            this.listBoxKolmogorovGaborPredict.Location = new System.Drawing.Point(763, 239);
            this.listBoxKolmogorovGaborPredict.Name = "listBoxKolmogorovGaborPredict";
            this.listBoxKolmogorovGaborPredict.Size = new System.Drawing.Size(406, 199);
            this.listBoxKolmogorovGaborPredict.TabIndex = 7;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1181, 450);
            this.Controls.Add(this.listBoxKolmogorovGaborPredict);
            this.Controls.Add(this.listBoxDefaultPredict);
            this.Controls.Add(this.groupBoxActions);
            this.Controls.Add(this.listBoxKolmogorovGabor);
            this.Controls.Add(this.listBoxDefault);
            this.Name = "Form";
            this.Text = "Лабораторная работа №1, Физ-тех, ИВТ-1, 4 курс, Мартынов В.В.,  САиУИС";
            this.groupBoxActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button buttonSelectData;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ListBox listBoxDefault;
        private System.Windows.Forms.ListBox listBoxKolmogorovGabor;
        private System.Windows.Forms.GroupBox groupBoxActions;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonSelectTarget;
        private System.Windows.Forms.Button buttonPredict;
        private System.Windows.Forms.ListBox listBoxDefaultPredict;
        private System.Windows.Forms.ListBox listBoxKolmogorovGaborPredict;
    }
}

