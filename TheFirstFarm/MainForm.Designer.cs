using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared.Serialization;
using TheFirstFarm.Utilities;

namespace TheFirstFarm {
	partial class MainForm {
		/// <summary>
		///     Required designer variable.
		/// </summary>
		private IContainer components = null;

		private TabControl TabControl;

		private Timer TimeLeftUpdater;

		private readonly List<Synchronizer> Synchronizers = new();

		private void InitializeComponent() {
			components = new Container();
			TabControl = new TabControl() {
				Name = "TabControl",
				Dock = DockStyle.Fill,
				Font = new Font("等线", 10.8F, FontStyle.Bold, GraphicsUnit.Point),
				ItemSize = new Size(96, 30),
				Location = new Point(0, 0),
				SelectedIndex = 0,
				Size = new Size(582, 553),
				SizeMode = TabSizeMode.Fixed,
				TabIndex = 0
			};
			Controls.Add(TabControl);
			foreach (var model in Enum.GetValues<SyncModel>()) {
				var synchronizer = new Synchronizer(model, this);
				Synchronizers.Add(synchronizer);
				TabControl.TabPages.Add(synchronizer.Page);
			}
			TimeLeftUpdater = new(components) { Enabled = true, Interval = 1000 };
			TimeLeftUpdater.Tick += (_, _) => UpdateTimeLeft();
			AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(582, 553);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			Name = "MainForm";
			Text = "同步器";
			Icon = Resource.Icon;
		}

		/// <summary>
		///     Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null))
				components.Dispose();
			base.Dispose(disposing);
		}

		internal partial class Synchronizer {
			#region Controls

			internal TabPage Page = new();

			internal Label Label1 = new();

			internal Label Label2 = new();

			internal Label Label3 = new();

			internal Label Label4 = new();

			internal Label Label5 = new();

			internal Label Label6 = new();

			internal RichTextBox LogTextBox = new();

			internal Button StopSyncButton = new();

			internal Button StartSyncButton = new();

			internal NumericUpDown SyncIntervalPicker = new();

			internal Button ManualSyncButton = new();

			internal DateTimePicker EndDatePicker = new();

			internal DateTimePicker StartDatePicker = new();

			internal TextBox TimeLeftBox = new();

			internal ContextMenuStrip LogTextBoxMenu = new();

			internal SaveFileDialog SaveDialog = new();

			#endregion
			internal SyncModel Model;

			internal void InitializeComponent() {
				// 
				// Page
				// 
				Page.Controls.Add(Label1);
				Page.Controls.Add(Label2);
				Page.Controls.Add(Label3);
				Page.Controls.Add(Label4);
				Page.Controls.Add(Label5);
				Page.Controls.Add(Label6);
				Page.Controls.Add(SyncIntervalPicker);
				Page.Controls.Add(TimeLeftBox);
				Page.Controls.Add(StopSyncButton);
				Page.Controls.Add(StartSyncButton);
				Page.Controls.Add(ManualSyncButton);
				Page.Controls.Add(EndDatePicker);
				Page.Controls.Add(StartDatePicker);
				Page.Controls.Add(LogTextBox);
				Page.Location = new Point(4, 34);
				Page.Margin = new Padding(0);
				Page.Name = $"{Model}Page";
				Page.Padding = new Padding(20);
				Page.Size = new Size(574, 515);
				Page.TabIndex = 0;
				Page.Text = Model.GetValue();
				Page.UseVisualStyleBackColor = true;
				// 
				// Label3
				// 
				Label3.AutoSize = true;
				Label3.Location = new Point(302, 32);
				Label3.Name = "Label3";
				Label3.Size = new Size(105, 22);
				Label3.TabIndex = 7;
				Label3.Text = "同步间隔：";
				Label3.SendToBack();
				// 
				// SyncIntervalPicker
				// 
				SyncIntervalPicker.Location = new Point(401, 30);
				SyncIntervalPicker.Margin = new Padding(10);
				SyncIntervalPicker.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
				SyncIntervalPicker.Name = "SyncIntervalPicker";
				SyncIntervalPicker.Size = new Size(94, 28);
				SyncIntervalPicker.TabIndex = 6;
				SyncIntervalPicker.TextAlign = HorizontalAlignment.Center;
				SyncIntervalPicker.Value = new decimal(new int[] { 1, 0, 0, 0 });
				// 
				// Label4
				// 
				Label4.AutoSize = true;
				Label4.Location = new Point(496, 32);
				Label4.Name = "Label4";
				Label4.Size = new Size(48, 22);
				Label4.TabIndex = 8;
				Label4.Text = "分钟";
				Label4.SendToBack();
				// 
				// Label5
				// 
				Label5.AutoSize = true;
				Label5.Location = new Point(302, 83);
				Label5.Name = "Label5";
				Label5.Size = new Size(143, 22);
				Label5.TabIndex = 11;
				Label5.Text = "距下次同步还剩";
				Label5.SendToBack();
				// 
				// TimeLeftBox
				// 
				TimeLeftBox.Location = new Point(444, 80);
				TimeLeftBox.Name = "TimeLeftBox";
				TimeLeftBox.ReadOnly = true;
				TimeLeftBox.Size = new Size(75, 28);
				TimeLeftBox.TabIndex = 12;
				TimeLeftBox.TextAlign = HorizontalAlignment.Center;
				// 
				// Label6
				// 
				Label6.AutoSize = true;
				Label6.Location = new Point(520, 83);
				Label6.Name = "Label6";
				Label6.Size = new Size(24, 22);
				Label6.TabIndex = 13;
				Label6.Text = "秒";
				Label6.SendToBack();
				// 
				// StartSyncButton
				// 
				StartSyncButton.Location = new Point(302, 126);
				StartSyncButton.Margin = new Padding(10);
				StartSyncButton.Name = "StartSyncButton";
				StartSyncButton.Size = new Size(115, 40);
				StartSyncButton.TabIndex = 9;
				StartSyncButton.Text = "开始同步";
				StartSyncButton.UseVisualStyleBackColor = true;
				StartSyncButton.Click += (_, _) => StartSync();
				// 
				// StopSyncButton
				// 
				StopSyncButton.Location = new Point(429, 126);
				StopSyncButton.Margin = new Padding(10);
				StopSyncButton.Name = "StopSyncButton";
				StopSyncButton.Size = new Size(115, 40);
				StopSyncButton.TabIndex = 10;
				StopSyncButton.Text = "停止同步";
				StopSyncButton.UseVisualStyleBackColor = true;
				StopSyncButton.Enabled = false;
				StopSyncButton.Click += (_, _) => StopSync();
				// 
				// ManualSyncButton
				// 
				ManualSyncButton.Location = new Point(30, 126);
				ManualSyncButton.Margin = new Padding(10);
				ManualSyncButton.Name = "ManualSyncButton";
				ManualSyncButton.Size = new Size(242, 40);
				ManualSyncButton.TabIndex = 5;
				ManualSyncButton.Text = "手动同步";
				ManualSyncButton.UseVisualStyleBackColor = true;
				ManualSyncButton.Click += (_, _) => ManualSync();
				// 
				// EndDatePicker
				// 
				EndDatePicker.Location = new Point(72, 78);
				EndDatePicker.Margin = new Padding(10);
				EndDatePicker.Name = "EndDatePicker";
				EndDatePicker.Size = new Size(200, 28);
				EndDatePicker.TabIndex = 4;
				// 
				// StartDatePicker
				// 
				StartDatePicker.Location = new Point(72, 30);
				StartDatePicker.Margin = new Padding(10);
				StartDatePicker.Name = "StartDatePicker";
				StartDatePicker.Size = new Size(200, 28);
				StartDatePicker.TabIndex = 3;
				// 
				// Label2
				// 
				Label2.AutoSize = true;
				Label2.Location = new Point(30, 83);
				Label2.Name = "Label2";
				Label2.Size = new Size(29, 22);
				Label2.TabIndex = 2;
				Label2.Text = "到";
				Label3.SendToBack();
				// 
				// Label1
				// 
				Label1.AutoSize = true;
				Label1.Location = new Point(30, 35);
				Label1.Name = "Label1";
				Label1.Size = new Size(29, 22);
				Label1.TabIndex = 1;
				Label1.Text = "从";
				Label1.SendToBack();
				// 
				// LogTextBox
				// 
				LogTextBox.Location = new Point(30, 186);
				LogTextBox.Margin = new Padding(10);
				LogTextBox.Name = "LogTextBox";
				LogTextBox.Size = new Size(514, 299);
				LogTextBox.TabIndex = 0;
				LogTextBox.Text = "";
				LogTextBox.ReadOnly = true;
				LogTextBox.Font = new Font("微软雅黑", 9);
				LogTextBox.ContextMenuStrip = LogTextBoxMenu;
				//
				// LogTextBoxMenu
				//
				LogTextBoxMenu.Items.Add("清空", null, (sender, e) => LogTextBox.Text = "");
				LogTextBoxMenu.Items.Add(
					"导出到文件",
					null,
					(sender, e) => {
						SaveDialog.Title = "保存日志文件";
						SaveDialog.Filter = "文本文件|*.txt|日志文件|*.log";
						SaveDialog.FileName = $"{Model.GetValue()}同步日志";
						if (SaveDialog.ShowDialog() == DialogResult.OK) {
							var writer = new StreamWriter(SaveDialog.OpenFile());
							writer.WriteLine(LogTextBox.Text);
							writer.Close();
						}
					}
				);
				var item = new ToolStripMenuItem("消息发送级别");
				foreach (var level in Enum.GetValues<LogLevel>()) {
					var checkItem = new ToolStripMenuItem(level.GetValue());
					checkItem.CheckOnClick = true;
					checkItem.Checked = (byte)level >= (byte)LogLevel.Warning;
					var lv = level;
					if (checkItem.Checked)
						MessageLevels.Add(lv);
					checkItem.CheckedChanged += (sender, e) => {
						if (checkItem.Checked)
							MessageLevels.Add(lv);
						else
							MessageLevels.Remove(lv);
					};
					item.DropDownItems.Add(checkItem);
				}
				LogTextBoxMenu.Items.Add(item);
			}
		}
	}

	public enum SyncModel : byte {
		[EnumValue("客户")]
		Customer,

		[EnumValue("销售订单")]
		SalesOrder,

		[EnumValue("发货单")]
		DeliveryOrder,

		[EnumValue("退换货单")]
		ReturnOrder,

		[EnumValue("发票")]
		Invoice,

		[EnumValue("产品")]
		Product
	}
}