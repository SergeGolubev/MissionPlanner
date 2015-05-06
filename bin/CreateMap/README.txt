Alpha version of CMPMVS v0.6.0.

· Complete binary distribution package for 64-bit Windows 7 OS with CUDA

CMPMVS - Multi-View Reconstruction Software

Authors: Michal Jancosek & Tomas Pajdla
Software written by: Michal Jancosek
Latest version: 0.6.0
Release date: September 28, 2012
Reference to cite: 
[1] M. Jancosek, T. Pajdla. Multi-View Reconstruction Preserving Weakly-Supported Surfaces, CVPR 2011 - IEEE Conference on Computer Vision and Pattern Recognition 2011
http://cmp.felk.cvut.cz/~jancom1/JancosekCVPR11.pdf 


Introduction
CMPMVS is a multi-view reconstruction software. The input to our software is a set of perspective images and camera parameters (internal and external camera calibrations). The output is a textured mesh of the rigid scene visible in the images. Non-rigid objects are implicitly ignored.

Dependences

Download and install Microsoft Visual Studio C++ 2008 x64 redistributable package (http://www.microsoft.com/download/en/details.aspx?displaylang=en&id=15336) along with the DirectX August 2009 redistributable package (http://www.microsoft.com/download/en/details.aspx?id=6883) and lastest GPU driver (it has to support CUDA 3.1 at least)!


Terms and Conditions
CMPMVS software is provided for research purposes only. In case you use this software for a publication, cite reference [1], please. See the LICENSE file provided with the demo software.


Third-Party Software
The binary depens on the following third-party software (note that all needed libraries are included in the distribution zip file already):
	OpenCV
	 · http://opencv.willowgarage.com/wiki/
	 · licensed under the BSD license
	CGAL
	 · http://www.cgal.org/download.html
	 · licensed under the Open Source License
	SiftGPU
	 · http://cs.unc.edu/~ccwu/siftgpu/
	zlib
	 · http://www.zlib.net/
	 · licensed under the following license
	maxflow-v3.01
	 · http://www.cs.ucl.ac.uk/staff/V.Kolmogorov/software.html


Directory contains :
ini\default.ini		 : the ini file with default parameters
ini\defaultLionHanau.ini : the ini file with default parameters for lionHanau.zip dataset from http://ptak.felk.cvut.cz/sfmservice/websfm.pl?menu=cmpmvs
ini\README.txt		 : the file explaining some of the parameters used in the ini files
visualSfM_CMPMVS.bat 	 : for complete 3D reconstruction just from images. You need to have instaled VisualSfM (http://www.cs.washington.edu/homes/ccwu/vsfm/) version 0.5.18 (or higher).


Input data format:
Input data are images and camera matrices. All images must be in one directory. All images must be the same resolution and must be the same format. Supported formats are: jpg,png,ppm,bmp,gif. Image name suffix must be five digits number. N-images must be numbered from 1 to n. Camera matrix for an image must be in txt file with the name imageName_P.txt where imageName is the name of the image. 

Example of 3 input files names:
00001.png
00001_P.txt
00002.png
00002_P.txt
00003.png
00003_P.txt

Example of camera matrix file content:
-105.345 -3146 -137.149 -24575.2 
-1154.97 -563.355 2646.3 -13219.2 
-0.887537 -0.449183 -0.102528 -9.84483 

Input: default ini files contains the following parameters to set for each new dataset:
[global]
dirName="fullDirPath\"			: The full path to the directory containing data (the '\' is needed!)
prefix=""				: Prefix of the images names (default four digits number)
imgExt="png"				: extension of the images
ncams=6					: number of the images
width=3072				: width of the images
height=2048				: height of the images
workDirName="_tmp"			: the directory name that will contain application temporary data. The directory will be created as the subdirectory of the dirName directory

For more informations about parameters meaning in the ini file see ini\README.txt file.

Output:
The output will be in the directory : dirName+"_OUT\" 


To test the application run :
CMPMVS.exe ini\defaultLionHanau.ini

Notes: 
You MUST run it from the directory where CMPMVS.exe is located! 
You need to download the lionHanau.zip dataset from http://ptak.felk.cvut.cz/sfmservice/websfm.pl?menu=cmpmvs
You need to change dirName in defaultLionHanau.ini to the directory where the lionHanau.zip is extracted


For discussion on the software please visit our Google group at http://groups.google.com/group/cmpmvs 

Contacts
Michal Jancosek, CMP, Dept. of Cybernetics, FEE, CTU Prague, mailto: jancom1@cmp.felk.vut.cz
Tomas Pajdla, CMP, Dept. of Cybernetics, FEE, CTU Prague, mailto: pajdla@cmp.felk.cvut.cz

References
[1] Multi-View Reconstruction Preserving Weakly-Supported Surfaces, M. Jancosek, T. Pajdla, IEEE Conference on Computer Vision and Pattern Recognition 2011





