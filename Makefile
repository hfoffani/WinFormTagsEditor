
MONOPATH=/Library/Frameworks/Mono.framework/Libraries/mono/4.5/
MONO=/Library/Frameworks/Mono.framework/Versions/Current/bin/mono
NUNITCONSOLE=packages/NUnit.Console.3.*/tools/nunit3-console.exe
TESTRES=--noresult

XAMARINDIR="/Applications/Xamarin Studio.app"
MDTOOL=$(XAMARINDIR)/Contents/MacOS/mdtool

NUGET=nuget


build:
	$(MDTOOL) build

nupkg: build
	(cd WinFormTagsEditor && \
	rm -f WinFormTagsEditor.*.nupkg && \
	$(NUGET) pack WinFormTagsEditor.nuspec )

publish:
	(cd WinFormTagsEditor && \
	$(NUGET) push WinFormTagsEditor.*.nupkg )
