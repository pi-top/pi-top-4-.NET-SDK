# Install jupyter and jupter lab on piTop

To install Jupyter notebooks and Jupyter lab without affecting your environment first install and confivure python virtual environment

## Installing virtualenv module and creating environment

1. Install the virtualend module 
```sh
pi@pi-top:~ sudo apt install virtualenv -y
```
2.  Create a new virtual environment
```sh
pi@pi-top:~ virtualenv .jupyter_venv -p python3
```
3.  activate the environment
```sh
pi@pi-top:~ source .jupyter_venv/bin/activate
```

## Install jupyter notebook and jupyter lab

1. activate your virtual environment
```sh
pi@pi-top:~ source .jupyter_venv/bin/activate
```
2. install jupyter and then jupyter lab
```sh
(pi-top) pi@pi-top:~ pip install jupyter
(pi-top) pi@pi-top:~ pip install jupyterlab
```

## Install dotnet interactive kernels

1. make sure you have [installed the dotnet sdk and dotnet interactive](install-dotnet-interactive)
2. activate your environment
```sh
pi@pi-top:~/source pi-top/bin/activate
```
3. install the kernels
```sh
(pi-top) pi@pi-top:~ dotnet interactive jupyter install
```

## Launching jupyter lab 

First activate the virtual environment
```sh
pi@pi-top:~/source pi-top/bin/activate
```

To run jupyter lab use the command
```sh
(pi-top) pi@pi-top:~ jupyter lab
```

To run the server on your pi-top and connect from another machine
```sh
(pi-top) pi@pi-top:~ jupyter lab --ip 0.0.0.0 --no-browser
```

The output will be similar to
```sh
[I 14:21:39.480 LabApp] JupyterLab extension loaded from /home/pi/pi-top/lib/python3.7/site-packages/jupyterlab
[I 14:21:39.480 LabApp] JupyterLab application directory is /home/pi/pi-top/share/jupyter/lab
[I 14:21:39.486 LabApp] Serving notebooks from local directory: /home/pi/
[I 14:21:39.487 LabApp] The Jupyter Notebook is running at:
[I 14:21:39.487 LabApp] http://pi-top:8888/?token=72376229c085c52fdf572283604d955ccf37810c8435f2bb
[I 14:21:39.487 LabApp]  or http://127.0.0.1:8888/?token=72376229c085c52fdf572283604d955ccf37810c8435f2bb
[I 14:21:39.487 LabApp] Use Control-C to stop this server and shut down all kernels (twice to skip confirmation).
[C 14:21:39.503 LabApp] 
    
    To access the notebook, open this file in a browser:
        file:///home/pi/.local/share/jupyter/runtime/nbserver-25169-open.html
    Or copy and paste one of these URLs:
        http://pi-top:8888/?token=72376229c085c52fdf572283604d955ccf37810c8435f2bb
     or http://127.0.0.1:8888/?token=72376229c085c52fdf572283604d955ccf37810c8435f2bb

```

Find the ip your pi-top is using (10.0.0.10 in the example) and open a browser at
```http://10.0.0.10:8888/?token=72376229c085c52fdf572283604d955ccf37810c8435f2bb```