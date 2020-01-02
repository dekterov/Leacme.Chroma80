// Copyright (c) 2017 Leacme (http://leac.me). View LICENSE.md for more information.
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FFmpeg.NET;
using FFmpeg.NET.Enums;
using MMTools;
using MMTools.Runners;

namespace Leacme.Lib.Chroma80 {

	public class Library {

		public Engine ffmpeg { get; }
		public string TempOutputFilepath { get; set; } = Path.Combine(Path.GetTempPath(), typeof(Library).Namespace + ".temp.mp4");

		public Library() {

			MMToolsConfiguration.Register();
			var ffRunner = new MMRunner(MMAppType.FFMPEG);
			var pathField = (string)ffRunner.GetType().GetProperty("ApplicationPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ffRunner);

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				pathField = pathField + ".exe";
			}

			ffmpeg = new Engine(pathField);
		}

		/// <summary>
		/// Retrieves the metadata of the input media file. Currently dummy runs the ffmpeg process as a workaround.
		/// /// </summary>
		/// <param name="inputFilepath">Path to the media file.</param>
		/// <returns>MetaData object if the file is a valid media file that can be read by ffmpeg.</returns>
		public async Task<MetaData> GetFileMetadata(Uri inputFilepath) {
			MediaFile inputFile = null;
			try {
				inputFile = new MediaFile(inputFilepath.LocalPath);
			} catch (Exception e) {
				throw new ArgumentException("Invalid input file. " + e.Message);
			}
			if (inputFile == null) {
				throw new ArgumentException("Invalid input file.");
			}
			var tempOut1 = new MediaFile(TempOutputFilepath);
			var options = new ConversionOptions();
			options.CutMedia(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(3));
			try {
				await ffmpeg.ConvertAsync(inputFile, tempOut1, options);
			} catch (Exception e) {
				throw new ArgumentException("Invalid input file. " + e.Message);
			}
			var md = (MetaData)inputFile.GetType().GetProperty("MetaData", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(inputFile);
			if (md == null) {
				throw new ArgumentException("Invalid input file.");
			}

			File.Delete(tempOut1.FileInfo.FullName);
			return md;
		}

		/// <summary>
		/// Convert a media file via ffmpeg.
		/// /// </summary>
		/// <param name="inputFilepath">The input media file path.</param>
		/// <param name="outputFilepath">The output media file path.</param>
		/// <param name="outputFormat">The format to which to convert.</param>
		/// <param name="trimLength">Optional</param>
		/// <param name="trimFromPosition">Optional</param>
		/// <param name="audioBitRate">Optional</param>
		/// <param name="audioSampleRate">Optional</param>
		/// <param name="target">Optional</param>
		/// <param name="targetStandard">Optional</param>
		/// <param name="videoAspectRatio">Optional</param>
		/// <param name="videoBitRate">Optional</param>
		/// <param name="videoFps">Optional</param>
		/// <param name="videoSize">Optional</param>
		/// <param name="customWidth">Optional</param>
		/// <param name="customHeight">Optional</param>
		/// <param name="sourceCrop">Optional</param>
		/// <param name="baselineProfile">Optional</param>
		/// <returns></returns>
		public async Task ConvertFile(Uri inputFilepath, Uri outputFilepath, string outputFormat, TimeSpan? trimLength = null, TimeSpan? trimFromPosition = null,
			int? audioBitRate = null, AudioSampleRate? audioSampleRate = null, Target? target = null,
				TargetStandard? targetStandard = null, VideoAspectRatio? videoAspectRatio = null, int? videoBitRate = null,
					int? videoFps = null, VideoSize? videoSize = null, int? customWidth = null, int? customHeight = null,
					CropRectangle sourceCrop = null, bool? baselineProfile = null) {

			if (inputFilepath.LocalPath.Equals(outputFilepath.LocalPath + "." + outputFormat) || inputFilepath == null || outputFilepath == null) {
				throw new ArgumentException("Input and output paths cannot be the same or null.");
			}
			if (outputFormat == null) {
				throw new ArgumentException("Output format cannot be null.");
			}

			var inputFile = new MediaFile(inputFilepath.LocalPath);
			var outputFile = new MediaFile(outputFilepath.LocalPath + "." + outputFormat);
			var options = new ConversionOptions();

			if (trimFromPosition.HasValue && trimLength.HasValue) {
				options.CutMedia(trimFromPosition.Value, trimLength.Value);
			} else if (trimLength.HasValue) {
				options.CutMedia(TimeSpan.FromSeconds(0), trimLength.Value);
			}

			if (audioBitRate.HasValue) {
				options.AudioBitRate = audioBitRate.Value;
			}
			if (audioSampleRate.HasValue) {
				options.AudioSampleRate = audioSampleRate.Value;
			}
			if (target.HasValue) {
				options.Target = target.Value;
			}
			if (targetStandard.HasValue) {
				options.TargetStandard = targetStandard.Value;
			}
			if (videoAspectRatio.HasValue) {
				options.VideoAspectRatio = videoAspectRatio.Value;
			}
			if (videoBitRate.HasValue) {
				options.VideoBitRate = videoBitRate.Value;
			}
			if (videoFps.HasValue) {
				options.VideoFps = videoFps.Value;
			}
			if (videoSize.HasValue) {
				options.VideoSize = videoSize.Value;
			}
			if (customWidth.HasValue) {
				options.CustomWidth = customWidth.Value;
			}
			if (customHeight.HasValue) {
				options.CustomHeight = customHeight.Value;
			}
			if (sourceCrop != null) {
				options.SourceCrop = sourceCrop;
			}
			if (baselineProfile.HasValue) {
				options.BaselineProfile = baselineProfile.Value;
			}
			await ffmpeg.ConvertAsync(inputFile, outputFile, options);

		}
	}
}