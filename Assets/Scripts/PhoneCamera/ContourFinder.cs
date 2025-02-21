using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using Unity.VisualScripting;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.CoreModule;
using OpenCVForUnityExample;
using UnityEngine.UI;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;

public class ContourFinder : MonoBehaviour
{
    [SerializeField] private WebCamTexture webCamTexture;
    [SerializeField] private RawImage SourceRawImage;
    [SerializeField] private RawImage TargetRawImage;

    [SerializeField] private float curveAccuracy;

    CascadeClassifier cascade;

    private void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture(devices[1].name, 480, 640, 30);
        webCamTexture.Play();
        cascade = new CascadeClassifier(Application.dataPath + @"haarcascade_frontalface_default.xml");
    }

    private void Update()
    {
        
        ProcessTexture();

    }

    private void ProcessTexture()
    {
        Mat mainMat = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC3);

        //convert webcam texture to matrix
        Utils.webCamTextureToMat(webCamTexture, mainMat);

        Mat grayMat = new Mat();

        //copy main matrix to grayMat
        mainMat.copyTo(grayMat);

        //convert color to gray
        Imgproc.cvtColor(grayMat, grayMat, Imgproc.COLOR_BGR2GRAY);

        //blur the image
        Imgproc.GaussianBlur(grayMat, grayMat, new Size(5, 5), 0);

        //thresholding makes the image black and white
        Imgproc.threshold(grayMat, grayMat, 0, 255, Imgproc.THRESH_OTSU);

        //extract the edge o the image
        Imgproc.Canny(grayMat, grayMat, 50, 50);

        //Contours//

        //prep finding contours
        List<MatOfPoint> contours = new List<MatOfPoint>();
        
        //find the contour from the edges
        Imgproc.findContours(grayMat, contours, new Mat(), Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);

        for(int i = 0; i < contours.Count; i++)
        {
            MatOfPoint contourPoint = contours[i];
            MatOfPoint2f contourN = new MatOfPoint2f(contourPoint.toArray());
            double procLength = Imgproc.arcLength(contourN, true);

            MatOfPoint2f approx = new MatOfPoint2f();
            //convert contour to readable polygon
            Imgproc.approxPolyDP(contourN, approx, curveAccuracy, true);
            var area = Imgproc.contourArea(contourPoint);

            //find a contour
            MatOfPoint approxPoint = new MatOfPoint();
            approx.convertTo(approxPoint, CvType.CV_32S);

            
            
        }





        Texture2D finalTexture = new Texture2D(grayMat.width(), grayMat.height(), TextureFormat.RGB24, false);

        Utils.matToTexture2D(grayMat, finalTexture);

        SourceRawImage.texture = finalTexture;

        FindNewFace(grayMat);
    }

    private void FindNewFace(Mat grayMat)
    {
        //var faces = cascade.detectMultiScale(grayMat, 1.1, 2, HaarDetectionType.ScaleImage)
    }
}
