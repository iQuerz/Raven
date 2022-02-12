### Python Script for Extracting tasks form .cs files
##  Creator itsaghin

import os

def printFile(out, file, element):
    print("File name: " + element)
    print("---------------------------------")
    while out != "":
        if(out.find("TO DO") > -1 or out.find("TODO") > -1):
            print("     " + out[0:len(out)-1].strip())
        out = file.readline()
    print("---------------------------------\n")

def search(path):
    fullpath = path
    inDir = os.listdir(path)

    for element in inDir:
        if element.endswith(".cs") or element.endswith(".xaml") or element.endswith(".txt"):
            file = open(os.path.join(path, element), "r")
            isEmpty = True
            out = file.readline()
            while out != "":
                if(out.find("TO DO") > -1 or out.find("TODO") > -1):
                    isEmpty = False
                    break;
                out = file.readline()
            if(not isEmpty):
                printFile(out,file,element)
            if os.path.isdir(os.path.join(path, element)):
                return
        elif os.path.isdir(os.path.join(path, element)):
            fullpath = search(os.path.join(path, element))
            if fullpath is not None:
                return fullpath

search(".")
print("press any key to exit:")
input()
