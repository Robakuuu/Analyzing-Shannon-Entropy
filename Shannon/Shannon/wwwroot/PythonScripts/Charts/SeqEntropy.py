from pickle import TRUE
import matplotlib.pyplot as plt
import sys
from os.path import exists


class SeqEntropy:
    def __init__(self):
        self.args= sys.argv
        self.pathToFile= self.GetPathToFileFromArgs(self.args)
        self.Start(self.pathToFile)

    def Start(self,pathToFile):
        if not self.CheckFileExisitng(pathToFile):
            exit(0)
        datas=self.LoadFileData(pathToFile)
        datas=self.PrepareData(datas)
        title=self.GetFileName(pathToFile)
        self.GeneratePlot(datas,title)
        print("Plot generated.")
        
    def GetFileName(self,pathToFile:str):
        index= pathToFile.rfind("\\")
        return pathToFile
    def GeneratePlot(self,datas,title):
        xpoints = datas
        ypoints=[]
        for index in range(len(xpoints)):
            ypoints.append(index+1)
        plt.plot(ypoints,xpoints, linestyle = 'dotted')
        plt.title("Plot generated from: ")
        plt.savefig(title+'.jpg')
    def PrepareData(self,data):
        returnData =[]
        for item in data:
            if '\n' in item:
                item=item[:-1]
            returnData.append(float(item))
        return returnData
            
    def LoadFileData(self,filePath):
        file = open(filePath,"r")
        data = file.readlines()
        file.close()
        return data
    def GetPathToFileFromArgs(self,args):
        for arg in args:
            if ".py" not in arg:
                return  str(arg)
        return ""
    def CheckFileExisitng(self,filePath):
        if exists(filePath):
            return  True
        print("File doesnt exist.")
        return False
app = SeqEntropy()
