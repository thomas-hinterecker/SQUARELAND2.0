using UnityEngine;
using System.Collections;
using SquarelandSystem;

public class ScreenshotMovie : MonoBehaviour
{

	public string folder = "ScreenshotMovieOutput";

	public int frameRate = 25;

	public int sizeMultiplier = 1;

	protected Squareland squareland;

	private string realFolder = "";
	
	void Start () {

		squareland = (Squareland) Controller.procedure.GetCurrentTrial().GetCurrentCommand();

		if (true == squareland.record) {
			Time.captureFramerate = frameRate;

			if (squareland.recordFolderName != "") {
				folder = squareland.recordFolderName;
			}

			realFolder = folder;
			int count = 1;
			while (System.IO.Directory.Exists(realFolder)) {
				realFolder = folder + count;
				count++;
			}
			System.IO.Directory.CreateDirectory(realFolder);
		}
	}
	
	void Update () {
		if (true == squareland.record) {
			var name = string.Format("{0}/shot {1:D04}.png", realFolder, Time.frameCount);

			Application.CaptureScreenshot(name, sizeMultiplier);
		}
	}
}