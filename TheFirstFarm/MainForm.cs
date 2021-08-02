using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kingdee;
using Shared.Exceptions;
using TheFirstFarm.Utilities;
using FClient = FXiaoKe.Client;
using KClient = Kingdee.Client;

namespace TheFirstFarm {
	public partial class MainForm : Form {
		public MainForm() {
			var fSection = (NameValueCollection)ConfigurationManager.GetSection("fXiaoKe");
			var kSection = (NameValueCollection)ConfigurationManager.GetSection("kingdee");
			FClient = new FClient(fSection["appId"], fSection["appSecret"], fSection["permanentCode"]);
			KClient = new KClient(kSection["url"], kSection["databaseId"], kSection["username"], kSection["password"], (Language)Convert.ToInt32(kSection["languageId"]));
			InitializeComponent();
		}

		public FClient FClient { get; }

		public KClient KClient { get; }

		internal void UpdateTimeLeft() {
			var activeSync = Synchronizers.FirstOrDefault(sync => sync.Page.TabIndex == TabControl.SelectedIndex);
			if (activeSync is not null && activeSync.SyncTimer.Enabled && activeSync.LastSyncTime.HasValue) {
				var span = activeSync.LastSyncTime.Value + TimeSpan.FromMilliseconds(activeSync.SyncTimer.Interval) - DateTime.Now;
				activeSync.TimeLeftBox.Text = (span.TotalSeconds >= 0 ? Convert.ToInt32(span.TotalSeconds) : activeSync.SyncTimer.Interval).ToString();
			}
		}

		internal partial class Synchronizer {
			internal void ManualSync() {
				if (EndDatePicker.Value <= StartDatePicker.Value)
					MessageBox.Show(@"终止时间应该晚于起始时间", @"数据非法", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else {
					Synchronizing = true;
					LogTextBox.Focus();
					var _ = Synchronize(StartDatePicker.Value, EndDatePicker.Value)
						.ContinueWith(_ => Synchronizing = false);
				}
			}

			internal void StartSync() {
				SyncTimer.Interval = Convert.ToInt32(SyncIntervalPicker.Value * 60 * 1000);
				AutoSync();
				SyncTimer.Start();
				StartSyncButton.Enabled = false;
				StopSyncButton.Enabled = true;
				LogTextBox.Focus();
			}

			internal void StopSync() {
				SyncTimer.Stop();
				StartSyncButton.Enabled = true;
				StopSyncButton.Enabled = false;
			}

			internal void AutoSync() {
				if (Synchronizing)
					Tasks.Enqueue(
						new Task(
							() => {
								var now = DateTime.Now;
								Synchronize(LastSyncTime, now)
									.ContinueWith(
										_ => {
											LastSyncTime = now;
											Synchronizing = false;
										}
									);
							}
						)
					);
				else {
					Synchronizing = true;
					var now = DateTime.Now;
					var _ = Synchronize(LastSyncTime, now)
						.ContinueWith(
							_ => {
								LastSyncTime = now;
								Synchronizing = false;
							}
						);
				}
			}

			internal Task Synchronize(DateTime? start, DateTime? end = null) {
				end ??= DateTime.Now;
				return Model switch {
					SyncModel.Customer      => SyncCustomer(start, end.Value),
					SyncModel.SalesOrder    => SyncSalesOrder(start, end.Value),
					SyncModel.DeliveryOrder => SyncDeliveryOrder(start, end.Value),
					SyncModel.ReturnOrder   => SyncReturnOrder(start, end.Value),
					SyncModel.Invoice       => SyncInvoice(start, end.Value),
					SyncModel.Product       => SyncProduct(start, end.Value),
					_                       => throw new EnumValueOutOfRangeException()
				};
			}

			internal async Task SyncCustomer(DateTime? start, DateTime end) {
				Log("开始同步");
				await Task.Delay(1000);
				Log("同步结束");
			}

			internal async Task SyncSalesOrder(DateTime? start, DateTime end) { }

			internal async Task SyncDeliveryOrder(DateTime? start, DateTime end) { }

			internal async Task SyncReturnOrder(DateTime? start, DateTime end) { }

			internal async Task SyncInvoice(DateTime? start, DateTime end) { }

			internal async Task SyncProduct(DateTime? start, DateTime end) { }

			internal void Log(string message, LogLevel logLevel = LogLevel.Information, bool logTime = true) {
				var color = logLevel switch {
					LogLevel.Trace       => Color.DimGray,
					LogLevel.Debug       => Color.DodgerBlue,
					LogLevel.Information => Color.MediumSpringGreen,
					LogLevel.Warning     => Color.Gold,
					LogLevel.Error       => Color.Red,
					LogLevel.Critical    => Color.Magenta,
					_                    => throw new EnumValueOutOfRangeException()
				};
				if (logTime)
					LogTextBox.AppendText(DateTime.Now.ToString("HH:mm:ss.fff "), Color.Black);
				LogTextBox.AppendLine(message, color);
			}
		}
	}

	public enum LogLevel : byte {
		Trace,

		Debug,

		Information,

		Warning,

		Error,

		Critical
	}
}