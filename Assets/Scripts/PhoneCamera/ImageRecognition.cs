using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Sentis;
using HoloLab.DNN.Classification;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;

public class ImageRecognition : MonoBehaviour
{
    [SerializeField] private WebCamTexture webCamTexture;
    [SerializeField] private RawImage SourceRawImage;
    [SerializeField] private RawImage TargetRawImage;
    [SerializeField] private Texture2D inputTexture;

    private void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture(devices[1].name, 480, 640, 30);
        webCamTexture.Play();
    }

    private void Update()
    {
        
        ProcessTexture();

    }

    private void ProcessTexture()
    {
        Mat mainMat = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC3);

        //convert webcam to texture2d
        inputTexture = GetTexture2DFromWebcam(webCamTexture);

        //convert webcam texture to matrix
        Utils.webCamTextureToMat(webCamTexture, mainMat);

        Mat grayMat = new Mat();

        //copy main matrix to grayMat
        mainMat.copyTo(grayMat);

        //convert color to gray
        //Imgproc.cvtColor(grayMat, grayMat, Imgproc.COLOR_BGR2GRAY);

        //blur the image
        //Imgproc.GaussianBlur(grayMat, grayMat, new Size(5, 5), 0);

        //thresholding makes the image black and white
        //Imgproc.threshold(grayMat, grayMat, 0, 255, Imgproc.THRESH_OTSU);

        //extract the edge o the image
        //Imgproc.Canny(grayMat, grayMat, 50, 50);

        //Contours//

        //prep finding contours
        //List<MatOfPoint> contours = new List<MatOfPoint>();
        
        //find the contour from the edges
        //Imgproc.findContours(grayMat, contours, new Mat(), Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);




        Texture2D finalTexture = new Texture2D(grayMat.width(), grayMat.height(), TextureFormat.RGB24, false);

        Utils.matToTexture2D(grayMat, finalTexture);

        SourceRawImage.texture = finalTexture;

        //free up texture memory
        Destroy(inputTexture);
    }

    public Texture2D GetTexture2DFromWebcam(WebCamTexture webCamTexture)
    {
        Texture2D texture2D = new Texture2D(webCamTexture.width, webCamTexture.height);
        texture2D.SetPixels(webCamTexture.GetPixels());
        texture2D.Apply();
        return texture2D;
    }
}
