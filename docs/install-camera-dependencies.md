# Install camera library dependencies

To use the piTop.Camera pacakge you need to install the following dependencies first

## Install openCV

Get the file ```opencv-dotnet``` from the [libs](https://github.com/pi-top/pi-top-4-.NET-SDK/tree/master/libs) folder

```sh
sudo unzip -d /usr/local/lib/ ../libs/opencv-dotnet-4.5.0.zip
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