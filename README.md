# pi-top
.NET core api for pi-

The src directory contains the code for libraries you can use to create .NET core app for the amazing [pi-top4 paltform](https://www.pi-top.com/products/pi-top-4). Get one and get creative.

The libraries comes along with [dotnet interactive](https://github.com/dotnet/interactive/) intergration so you can use notebooks to explore the power of pi-top.

To use the notebook sample you will need jupyter and the dotnet interactive tool installed, if you don't have them follow this instructions

 * create and activate a virtual environment 
 * install jupyter and jupyter lab module in the environment using pip
 * install the dotnet interactive tool as shown [here](https://github.com/dotnet/interactive/)
 * install the dotnet interactive kernels with the command ```dotnet interactive jupyter install --http-port-range STARTPORT-ENDPORT``` 

Now to use the notebook

 * build the project ```>dotnet build```
 * create folder ```/home/pi/localNuget```
 * pack the proejcts with ```> dotnet pack /p:PackageVersion=1.1.1 -o /home/pi/localNuget```
  

Look at this example.
  
![image](https://user-images.githubusercontent.com/375556/80700336-71322400-8ad5-11ea-8eb1-6122c9cac554.png)
