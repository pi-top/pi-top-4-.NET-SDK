# Install camera library dependencies

To use the piTop.Camera pacakge you need to install the following dependencies first

## Install openCV

Get the file ```opencv_opencvsharp_libs.tar.gz``` from the [libs](https://github.com/colombod/pi-top/tree/master/libs) folder

```sh
sudo tar -xzvf opencv_opencvsharp_libs.tar.gz -C /usr/local/lib/
export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/usr/local/lib
sudo ldconfig
```

Add the following to .bashrc
```conf
export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/usr/local/lib
```

## Install System.Drawing dependecies

Install native dependencies 
```sh
sudo apt install libc6-dev 
sudo apt install libgdiplus
```

You can read more details on this [Scott Hanselman blog](https://www.hanselman.com/blog/HowDoYouUseSystemDrawingInNETCore.aspx)