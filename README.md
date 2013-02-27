OpenWSCF2010
============

This is a port from [patterns &amp; practices Web Client Developer Guidance](http://webclientguidance.codeplex.com/wikipage?title=Web%20Client%20Software%20Factory&referringTitle=Home) package to run WCSF on VS2012

WCSF2010-VS2012.vsix is a new package built from the original code in order to be used with Visual Studio 2012.<br>
It depends on OpenGAX2010 and a refreshed built of GEL 2.0.0.0
None of the sources were changed (GAX, GEL or WCSF). I just refreshed the dependencies and at most updated config files as recomendedd on OpenGAX documentation.

All the sources are also available in the repository.<br>
All the dependencies are already refreshed and Web Client Software Factory 2010 Source should be all ready to be build again if needed.


_________________________________________

Full steps to port Web Client Software Factory 2010 to VS2012


  1) download aind install [OpenGAX and OpenGAT](http://opengax.codeplex.com/releases/view/89857)
  
  
  2) download and install [VS2012 SDK](http://www.microsoft.com/en-us/download/details.aspx?id=30668)
  
  3) download [Web Client Software Factory 2010 Source](http://webclientguidance.codeplex.com/releases/view/43000)
  
  4) download [GEL for GAX2010](http://gel.codeplex.com/releases/view/45475)
  
  5) build "GEL for GAX2010" and make sure all the GAX related dependencies are pointing to version 2.0.0.0		

  6) open "Web Client Software Factory 2010 Source" 
  (\sources\WCSF2010\Trunk\Source\WebClientFactoryGP) 

  7) make sure all the GAX related dependencies are pointing to OpenGAX 
  (version=2.0.0.0, PublicKeyToken=023ca9fed18f34f0)

  8) make sure that GEL reference(Microsoft.Practices.RecipeFramework.Extensions.dll) is pointing to the version you just built

  9) follow OpenGAX documentation [Porting existing packages to the open source GAX](http://opengax.codeplex.com/wikipage?title=Porting%20existing%20packages%20to%20the%20open%20source%20GAX&referringTitle=Documentation) and [Porting existing packages to Visual Studio 2012](http://opengax.codeplex.com/wikipage?title=Porting%20existing%20packages%20to%20Visual%20Studio%202012&referringTitle=Documentation)

  10) build  "Web Client Software Factory 2010 Source" and your good to go!
