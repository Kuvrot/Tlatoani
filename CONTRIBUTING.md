### This are some rules made in order to maintain the project coherent.

#### Copyright notice
If your contribution it's artistic, make sure that is your own work, public domain work. If it is under CC Attribution then you must give proper Credit.

#### Coding
* All variables must named in CamelCase style. with variables starting with lowerCase letters, and methods and classes with upper case letters.
* Always give preference to optimization and performance.

***

#### Adding buildings

* All buildings 3D models must be in .Obj
* The building mesh (default) must be rotated 135 in the Y axis
* The building hierarchy must be the following
  * > Parent object (this have the scripts, the obstacle, and other components)
      * > Default (Just has the mesh renderer)
          * > Canvas
              * > Slider

* If the building produce units then the hierarchy must be the following: 
  * > Parent object (this have the scripts, the obstacle, and other components)
      * > Default (Just has the mesh renderer)
          * > Canvas
              * > Progress image (A filled image)
              * > Slider
      * Its important to note that the Progress image always must be above the Slider




***



