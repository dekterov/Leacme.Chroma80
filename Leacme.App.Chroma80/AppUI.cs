// Copyright (c) 2017 Leacme (http://leac.me). View LICENSE.md for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using FFmpeg.NET.Enums;
using Leacme.Lib.Chroma80;

namespace Leacme.App.Chroma80 {

	public class AppUI {

		private StackPanel rootPan = (StackPanel)Application.Current.MainWindow.Content;
		private Library lib = new Library();

		private (StackPanel holder, TextBlock label, TextBox field, Button button) finp = App.HorizontalFieldWithButton;
		private (StackPanel holder, TextBlock label, TextBox field, Button button) fout = App.HorizontalFieldWithButton;
		private TextBlock stLab = App.TextBlock;
		private (StackPanel holder, TextBlock label, ComboBox comboBox) fctOf = App.ComboBoxWithLabel;
		private TextBox fctTpFl = App.TextBox;
		private TextBox fctVlFl = App.TextBox;
		private TextBox fctArbFl = App.TextBox;
		private (StackPanel holder, TextBlock label, ComboBox comboBox) fctAsr = App.ComboBoxWithLabel;
		private (StackPanel holder, TextBlock label, ComboBox comboBox) fctT = App.ComboBoxWithLabel;
		private (StackPanel holder, TextBlock label, ComboBox comboBox) fctTs = App.ComboBoxWithLabel;
		private (StackPanel holder, TextBlock label, ComboBox comboBox) fctVar = App.ComboBoxWithLabel;
		private TextBox fctVbrFl = App.TextBox;
		private TextBox fctVfpsFl = App.TextBox;
		private (StackPanel holder, TextBlock label, ComboBox comboBox) fctVs = App.ComboBoxWithLabel;
		private TextBox fctWFl = App.TextBox;
		private TextBox fctHFl = App.TextBox;
		private TextBox metDurFl = App.TextBox;
		private TextBox metVfFl = App.TextBox;
		private TextBox metCmFl = App.TextBox;
		private TextBox metFsFl = App.TextBox;
		private TextBox metBrFl = App.TextBox;
		private TextBox metFpsFl = App.TextBox;
		private TextBox metAfFl = App.TextBox;
		private TextBox metSrFl = App.TextBox;
		private TextBox metCoFl = App.TextBox;
		private TextBox metAbFl = App.TextBox;

		public AppUI() {

			rootPan.Spacing = -4;

			var fold1 = App.HorizontalStackPanel;
			var fold2 = App.HorizontalStackPanel;
			var fold3 = App.HorizontalStackPanel;
			var fold4 = App.HorizontalStackPanel;
			var fold5 = App.HorizontalStackPanel;

			var fmet1 = App.HorizontalStackPanel;
			var fmet2 = App.HorizontalStackPanel;
			var fmet3 = App.HorizontalStackPanel;

			var cBt = App.Button;

			stLab.TextAlignment = TextAlignment.Center;
			stLab.Text = "Initialized";

			fctOf.label.Text = "Convert to format (required):";
			fctOf.comboBox.Items = Formats.Extensions;
			fctOf.comboBox.SelectedItem = Formats.Extensions.First(z => z.Equals("mp4"));

			var fctOpt = App.TextBlock;
			fctOpt.TextAlignment = TextAlignment.Center;
			fctOpt.Text = "Optional Conversion Controls:";

			var fctTpLab = App.TextBlock;
			fctTpLab.Text = "Trim from position (seconds):";

			var fctVlLab = App.TextBlock;
			fctVlLab.Text = "Trim length (seconds):";

			var fctAbrLab = App.TextBlock;
			fctAbrLab.Text = "Audio bitrate (kb/s):";

			fctAsr.label.Text = "Sample rate:";
			fctAsr.comboBox.Items = Enum.GetValues(typeof(AudioSampleRate));

			fctT.label.Text = "Target:";
			fctT.comboBox.Items = Enum.GetValues(typeof(Target));

			fctTs.label.Text = "Target standard:";
			fctTs.comboBox.Items = Enum.GetValues(typeof(TargetStandard));

			fctVar.label.Text = "Video aspect ratio:";
			fctVar.comboBox.Width = 100;
			fctVar.comboBox.Items = Enum.GetValues(typeof(VideoAspectRatio));

			var fctVrbLab = App.TextBlock;
			fctVrbLab.Text = "Video bitrate (kb/s):";

			var fctVfpsLab = App.TextBlock;
			fctVfpsLab.Text = "Frames per second:";

			fctVs.label.Text = "Video size:";
			fctVs.comboBox.Width = 100;
			fctVs.comboBox.Items = Enum.GetValues(typeof(VideoSize));

			var fctWLab = App.TextBlock;
			fctWLab.Text = "Custom Width (pixels):";

			var fctHLab = App.TextBlock;
			fctHLab.Text = "Custom Height (pixels):";

			var metBl1 = App.TextBlock;
			metBl1.TextAlignment = TextAlignment.Center;
			metBl1.Text = "Input File Properties:";

			var metDurLab = App.TextBlock;
			metDurLab.Text = "Duration (seconds):";

			var metVfLab = App.TextBlock;
			metVfLab.Text = "Video format:";

			var metCmLab = App.TextBlock;
			metCmLab.Text = "Color model:";

			var metFsLab = App.TextBlock;
			metFsLab.Text = "Frame size:";

			var metBrLab = App.TextBlock;
			metBrLab.Text = "Video bitrate (kb/s):";

			var metFpsLab = App.TextBlock;
			metFpsLab.Text = "Frames per second:";

			var metAfLab = App.TextBlock;
			metAfLab.Text = "Audio format:";

			var metSrLab = App.TextBlock;
			metSrLab.Text = "Sample rate:";

			var metCoLab = App.TextBlock;
			metCoLab.Text = "Channel output:";

			var metAbLab = App.TextBlock;
			metAbLab.Text = "Audio bitrate (kb/s):";

			metDurFl.IsReadOnly = metVfFl.IsReadOnly = metCmFl.IsReadOnly = metFsFl.IsReadOnly = metBrFl.IsReadOnly = metFpsFl.IsReadOnly = metAfFl.IsReadOnly = metSrFl.IsReadOnly = metCoFl.IsReadOnly = metAbFl.IsReadOnly = true;
			metDurFl.Background = metVfFl.Background = metCmFl.Background = metFsFl.Background = metBrFl.Background = metFpsFl.Background = metAfFl.Background = metSrFl.Background = metCoFl.Background = metAbFl.Background = fctOpt.Background = metBl1.Background = Brushes.LightGray;

			finp.label.Text = "Input file:";
			finp.field.Width = 700;
			finp.field.IsReadOnly = true;
			finp.field.Watermark = "required";
			finp.button.Content = "Input...";
			finp.button.Click += async (z, zz) => {
				var ofp = await OpenFile();

				metDurFl.Text = metVfFl.Text = metCmFl.Text = metFsFl.Text = metBrFl.Text = metFpsFl.Text = metAfFl.Text = metSrFl.Text = metCoFl.Text = metAbFl.Text = string.Empty;
				if (ofp.Any()) {
					finp.field.Text = ofp.First();

					((App)Application.Current).LoadingBar.IsIndeterminate = true;
					await ParseMetadata();
					((App)Application.Current).LoadingBar.IsIndeterminate = false;
				}
			};

			fout.label.Text = "Output file:";
			fout.field.Width = 700;
			fout.field.IsReadOnly = true;
			fout.field.Watermark = "required";
			fout.button.Content = "Output...";
			fout.button.Click += async (z, zz) => { var sfp = await SaveFile(); fout.field.Text = sfp; };

			cBt.Content = "Begin Conversion";
			cBt.Click += async (z, zz) => {
				if (!string.IsNullOrWhiteSpace(finp.field.Text) && !string.IsNullOrWhiteSpace(fout.field.Text)) {
					((App)Application.Current).LoadingBar.IsIndeterminate = true;
					await InitConversion();
					((App)Application.Current).LoadingBar.IsIndeterminate = false;
				} else {
					stLab.Text = "Fill in the input and output fields.";
				}
			};

			finp.holder.HorizontalAlignment = fold1.HorizontalAlignment = fold2.HorizontalAlignment =
				fold3.HorizontalAlignment = fold4.HorizontalAlignment = fold5.HorizontalAlignment = fmet1.HorizontalAlignment =
					fmet2.HorizontalAlignment = fmet3.HorizontalAlignment = fout.holder.HorizontalAlignment =
						fctOf.holder.HorizontalAlignment = HorizontalAlignment.Center;

			fold1.Children.AddRange(new List<IControl> { fctTpLab, fctTpFl, fctVlLab, fctVlFl });
			fold2.Children.AddRange(new List<IControl> { fctAbrLab, fctArbFl, fctAsr.holder });
			fold3.Children.AddRange(new List<IControl> { fctT.holder, fctTs.holder });
			fold4.Children.AddRange(new List<IControl> { fctVar.holder, fctVrbLab, fctVbrFl, fctVfpsLab, fctVfpsFl, fctVs.holder });
			fold5.Children.AddRange(new List<IControl> { fctWLab, fctWFl, fctHLab, fctHFl });

			fmet1.Children.AddRange(new List<IControl> { metDurLab, metDurFl, metVfLab, metVfFl, metCmLab, metCmFl, metFsLab, metFsFl, });
			fmet2.Children.AddRange(new List<IControl> { metBrLab, metBrFl, metFpsLab, metFpsFl });
			fmet3.Children.AddRange(new List<IControl> { metAfLab, metAfFl, metSrLab, metSrFl, metCoLab, metCoFl, metAbLab, metAbFl });

			rootPan.Children.AddRange(new List<IControl> { finp.holder, fctOf.holder, new Control { Height = 10 }, metBl1, fmet1, fmet2, fmet3, new Control { Height = 10 }, fctOpt, fold1, fold2, fold3, fold4, fold5, new Control { Height = 10 }, fout.holder, new Control { Height = 7 }, cBt, new Control { Height = 7 }, stLab });

			lib.ffmpeg.Complete += (z, zz) => {
				Dispatcher.UIThread.Post(() => {
					if (!zz.Output.FileInfo.FullName.Equals(lib.TempOutputFilepath)) {
						stLab.Text = "File converted successfully!";
					}
				});
			};

			lib.ffmpeg.Progress += (z, zz) => {
				Dispatcher.UIThread.Post(() => {
					if (!zz.Output.FileInfo.FullName.Equals(lib.TempOutputFilepath)) {
						stLab.Text = "Processed: " + zz.ProcessedDuration.TotalSeconds + " out of " + metDurFl.Text + " seconds...";
					}
				});
			};
			lib.ffmpeg.Error += (z, zz) => {
				Dispatcher.UIThread.Post(() => {
					if (!zz.Output.FileInfo.FullName.Equals(lib.TempOutputFilepath)) {
						stLab.Text = zz.Exception.Message;
					}
				});
			};

		}

		private async Task InitConversion() {
			TimeSpan? trimFromPosition = null;
			if (!string.IsNullOrWhiteSpace(fctTpFl.Text) && double.TryParse(fctTpFl.Text, out var tempfctTpFl)) {
				trimFromPosition = TimeSpan.FromSeconds(tempfctTpFl);
			}

			TimeSpan? trimLength = null;
			if (!string.IsNullOrWhiteSpace(fctVlFl.Text) && double.TryParse(fctVlFl.Text, out var tempfctVlFl)) {
				trimLength = TimeSpan.FromSeconds(tempfctVlFl);
			}

			int? audioBitRate = null;
			if (!string.IsNullOrWhiteSpace(fctArbFl.Text) && int.TryParse(fctArbFl.Text, out var tempfctArbFl)) {
				audioBitRate = tempfctArbFl;
			}

			AudioSampleRate? audioSampleRate = null;
			if (fctAsr.comboBox.SelectedItem != null) {
				audioSampleRate = (AudioSampleRate)fctAsr.comboBox.SelectedItem;
			}

			Target? target = null;
			if (fctT.comboBox.SelectedItem != null) {
				target = (Target)fctT.comboBox.SelectedItem;
			}

			TargetStandard? targetStandard = null;
			if (fctTs.comboBox.SelectedItem != null) {
				targetStandard = (TargetStandard)fctTs.comboBox.SelectedItem;
			}

			VideoAspectRatio? videoAspectRatio = null;
			if (fctVar.comboBox.SelectedItem != null) {
				videoAspectRatio = (VideoAspectRatio)fctVar.comboBox.SelectedItem;
			}

			int? videoBitRate = null;
			if (!string.IsNullOrWhiteSpace(fctVbrFl.Text) && int.TryParse(fctVbrFl.Text, out var tempfctVbrFl)) {
				videoBitRate = tempfctVbrFl;
			}

			int? videoFps = null;
			if (!string.IsNullOrWhiteSpace(fctVfpsFl.Text) && int.TryParse(fctVfpsFl.Text, out var tempfctVfpsFl)) {
				videoFps = tempfctVfpsFl;
			}

			VideoSize? videoSize = null;
			if (fctVs.comboBox.SelectedItem != null) {
				videoSize = (VideoSize)fctVs.comboBox.SelectedItem;
			}

			int? customWidth = null;
			if (!string.IsNullOrWhiteSpace(fctWFl.Text) && int.TryParse(fctWFl.Text, out var tempfctWFl)) {
				customWidth = tempfctWFl;
			}

			int? customHeight = null;
			if (!string.IsNullOrWhiteSpace(fctHFl.Text) && int.TryParse(fctHFl.Text, out var tempfctHFl)) {
				customHeight = tempfctHFl;
			}

			CropRectangle sourceCrop = null;
			bool? baselineProfile = null;

			try {
				await lib.ConvertFile(
					inputFilepath: new Uri(finp.field.Text),
					outputFilepath: new Uri(fout.field.Text),
					outputFormat: (string)fctOf.comboBox.SelectedItem,
					trimLength: trimLength,
					trimFromPosition: trimFromPosition,
					audioBitRate: audioBitRate,
					audioSampleRate: audioSampleRate,
					target: target,
					targetStandard: targetStandard,
					videoAspectRatio: videoAspectRatio,
					videoBitRate: videoBitRate,
					videoFps: videoFps,
					videoSize: videoSize,
					customWidth: customWidth,
					customHeight: customHeight,
					sourceCrop: sourceCrop,
					baselineProfile: baselineProfile

				);
			} catch (Exception e) {
				stLab.Text = e.Message;
			}
		}

		private async Task ParseMetadata() {
			try {
				var md = await lib.GetFileMetadata(new Uri(finp.field.Text));
				stLab.Text = "File loaded - ready for conversion.";

				if (md.Duration != null) {
					metDurFl.Text = md.Duration.TotalSeconds.ToString();
				}
				if (md.VideoData != null) {
					if (md.VideoData.Format != null) {
						metVfFl.Text = md.VideoData.Format;
					}
					if (md.VideoData.ColorModel != null) {
						metCmFl.Text = md.VideoData.ColorModel;
					}
					if (md.VideoData.FrameSize != null) {
						metFsFl.Text = md.VideoData.FrameSize;
					}
					if (md.VideoData.BitRateKbs.HasValue) {
						metBrFl.Text = md.VideoData.BitRateKbs.Value.ToString();
					}
					metFpsFl.Text = md.VideoData.Fps.ToString();
				}

				if (md.AudioData != null) {
					if (md.AudioData.Format != null) {
						metAfFl.Text = md.AudioData.Format;
					}
					if (md.AudioData.SampleRate != null) {
						metSrFl.Text = md.AudioData.SampleRate;
					}
					if (md.AudioData.ChannelOutput != null) {
						metCoFl.Text = md.AudioData.ChannelOutput;
					}
					metAbFl.Text = md.AudioData.BitRateKbs.ToString();
				}

			} catch (Exception e) {
				stLab.Text = e.Message;
				finp.field.Text = string.Empty;
			}

		}

		private async Task<IEnumerable<string>> OpenFile() {
			var dialog = new OpenFileDialog() {
				Title = "Open File to Convert...",
				InitialDirectory = Directory.GetCurrentDirectory(),
				AllowMultiple = false,
			};
			var res = await dialog.ShowAsync(Application.Current.MainWindow);
			return (res?.Any() == true) ? res : Enumerable.Empty<string>();
		}

		private async Task<string> SaveFile() {
			var dialog = new SaveFileDialog() {
				Title = "Save Converted File...",
				InitialDirectory = Directory.GetCurrentDirectory(),
			};
			var res = await dialog.ShowAsync(Application.Current.MainWindow);
			return (!string.IsNullOrWhiteSpace(res)) ? res : string.Empty;
		}

	}
}