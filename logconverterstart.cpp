#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#define BUFFERSIZE 150
#define READMAX BUFFERSIZE - 1

#define FIRST_MENU 0
#define CALM_SIM 1
#define SECOND_MENU 2
#define CLAUST_SIM 3
#define HEIGHT_SIM 4

#define HEIGHT_FEAR 2
#define CLAUST_FEAR 3
#define USER_NUMBER 4
#define OUTPUT_ARG 5
#define MIN_ARGS 6
#define MAX_ARGS 7

#define DATA_HOLDER_SIZE 1000

#define INVALID 65535


const char* fearStates[5]={"VR-Fear0","VR-Fear1","VR-Fear2","VR-Fear3","VR-Fear4"};
	
const char* simNames[5]={"FirstMenu","Calm","SecondMenu","Claustrophobia","Acrophobia"};

int userNumber;

FILE* output;

const char* heightFear;
const char* claustFear;

struct dataHolder{
	double heartrate;
	double variability;
	int curSim;
};

void calcDiffsAndPrint(double avgRate,double avgVar, double heartrate, double var, int curSim)
{
	//Compute deviations from baseline
        double ratePercent; 
        double rateDiff;
            
        if(avgRate != INVALID)
        {
            ratePercent = (heartrate - avgRate)/(double)avgRate;
            rateDiff = heartrate - avgRate;
        }
        else{
            ratePercent = INVALID;
            rateDiff = INVALID;
        }
            
        double varPercent;
        double varDiff;
            
        if(avgVar != INVALID)
        {
            varPercent = (var - avgVar) / (double)avgVar;
            varDiff = var - avgVar;
        }
        else{
            varPercent = INVALID;
            varDiff = INVALID;
        }
			
	const char* curSimFear;
	if(curSim == CALM_SIM || curSim == FIRST_MENU)
		curSimFear = fearStates[0];
	else if(curSim == HEIGHT_SIM)
		curSimFear = heightFear;
	else if(curSim == CLAUST_SIM)
		curSimFear = claustFear;
	else{
		curSimFear = fearStates[0];
	}
			
            
	if(avgVar == INVALID || var == INVALID)
		fprintf(output,"%f,%f,?,?,%d,%s,%s\n",ratePercent,rateDiff,userNumber,simNames[curSim],curSimFear);
	else if(avgRate == INVALID || heartrate == INVALID)
		fprintf(output,"?,?,%f,%f,%d,%s,%s\n",varPercent,varDiff,userNumber,simNames[curSim],curSimFear);
	else{
		fprintf(output,"%f,%f,%f,%f,%d,%s,%s\n",ratePercent,rateDiff,varPercent,varDiff,userNumber,simNames[curSim],curSimFear);
	}


}

int main(int argc, char* argv[])
{
    if(argc < MIN_ARGS || argc > MAX_ARGS)
    {
        printf("The accepted arguments are:\n");
        printf("[log_file] [height_fear] [claust_fear] [user_number] [output_file] (optional_continued_log)\n");
        return 1;
    }
    
    FILE* file = fopen(argv[1],"r");
    
    if(file == NULL)
    {
        printf("Failed to open %s for reading!\n",argv[1]);
        return 1;
    }
    

    userNumber = atoi(argv[USER_NUMBER]);

    if(userNumber == 1)
    	output = fopen(argv[OUTPUT_ARG],"w");
    else
	output = fopen(argv[OUTPUT_ARG],"a");
    if(output == NULL)
    {
        printf("Failed to open %s for writing!\n",argv[OUTPUT_ARG]);
        return 1;
    }
    
    FILE* file2 = NULL;
    
    if(argc == MAX_ARGS)
    {
        file2 = fopen(argv[MAX_ARGS - 1],"r");
        if(file2 == NULL)
        {
            printf("Failed to open %s for reading!\n",argv[MAX_ARGS-1]);
            return 1;
        }
    }
    
    char buffer[150];
    
    memset(buffer,0,sizeof(char)*BUFFERSIZE);
    
	int heightFearInt = atoi(argv[HEIGHT_FEAR]);
	int claustFearInt = atoi(argv[CLAUST_FEAR]);
	

	
	
    //Average heart rate
    double avgRate = 0.0;
    //Average heart rate variability
    double avgVar = 0.0;
    //The simulation on the last line parsed
    int lastSim = FIRST_MENU;
    
    //Number of points used to get the baseline heart rate
    int baseRateCount = 0;
    //Number of points used to get the baseline variability
    int baseVarCount = 0;
    
    bool baselineDone = false;
    
    if(userNumber == 1)
    {
    	//Print out header section for ARFF format.
    	fprintf(output,"%% 1. Title: VR Fearless Data Log\n\n");

	fprintf(output,"@RELATION vrdata\n\n");
	fprintf(output,"@ATTRIBUTE ratePercent NUMERIC\n");
	fprintf(output,"@ATTRIBUTE rateDiff NUMERIC\n");
	fprintf(output,"@ATTRIBUTE variabilityPercent NUMERIC\n");
	fprintf(output,"@ATTRIBUTE variabilityDiff NUMERIC\n");	
	fprintf(output,"@ATTRIBUTE userNumber NUMERIC\n");
	fprintf(output,"@ATTRIBUTE currentSim {FirstMenu,Calm,SecondMenu,Claustrophobia,Acrophobia}\n");
	
	
	//Fear0 is no fear, Fear1 is minor fear, Fear2 is moderate (target) fear, Fear3 is strong fear, Fear4 is overly strong fear
	
	fprintf(output,"@ATTRIBUTE class {VR-Fear0, VR-Fear1, VR-Fear2, VR-Fear3, VR-Fear4}\n");
	
	fprintf(output,"@DATA\n");
    }
    


	heightFear = fearStates[heightFearInt];
	claustFear = fearStates[claustFearInt];
	
	dataHolder initialData[DATA_HOLDER_SIZE];

    printf("Now parsing %s...\n",argv[1]);
    
	fprintf(output,"%% File: %s\n",argv[1]);
	
    //Read in first line (comment line, no data)
    if(fgets(buffer,READMAX,file) == NULL)
    {
        printf("Error reading comment line from file! Aborting!\n");
        return 1;
    }
    
	int initialIter = 0;

    while(!feof(file))
    {
        //printf("Starting line.\n");
        //Read line from file
        if(fgets(buffer,READMAX,file) == NULL)
        {
            if(feof(file))
                break;
            
            printf("Error reading from file! Aborting!\n");
            return 1;
        }
        
        //printf("Read %s\n",buffer);
        
        //Parse line, extract heart rate, variability, confidence, and current simulation
        char* date;
        date = strtok(buffer,",");
        
        char* time;
        time = strtok(NULL,",");
        
        char* heartrateStr;
        heartrateStr = strtok(NULL,",");
        int heartrate = atoi(heartrateStr);
        
        char* varStr;
        varStr = strtok(NULL,",");
        int var = atoi(varStr);
        
        char* confidenceStr;
        confidenceStr = strtok(NULL,",");
        int confidence = atoi(confidenceStr);
        
        char* simStr;
        simStr = strtok(NULL,",");
        int curSim = atoi(simStr);
        
        //printf("%s,%s,%s,%s,%s,%s\n",date,time,heartrateStr,varStr,confidenceStr,simStr);
        
        //Do something with the data (depends on current simulation)
        if(curSim > CALM_SIM && !baselineDone)
        {
            //Just left the calm sim, compute the baselines
            
            //If there are less than 10 valid points, give a warning about an error.
            if(baseRateCount >= 10)
            {
                avgRate /= (double)baseRateCount;
            }
            else{
                printf("Warning: only %d valid points for heart rate baseline!\n",baseRateCount);
                printf("All heart rate data will be marked invalid to avoid bad data.\n");
                avgRate = INVALID;
            }
            
            //Same with heart rate variability
            if(baseVarCount >= 10)
            {
                avgVar /= (double)baseVarCount;
            }
            else{
                printf("Warning: only %d valid points for heart variability baseline!\n",baseVarCount);
                printf("All heart variability data will be marked invalid to avoid bad data.\n");
                avgVar = INVALID;
            }
			
			if(avgRate == INVALID && avgVar == INVALID)
			{
				printf("All data invalid! Aborting.\n");
				fclose(file);
				fclose(output);
				return 1;
			}
            
            fprintf(output,"%% average heart rate: %f, average heart variability: %f\n",avgRate,avgVar);
            
            baselineDone = true;

		
		//Add the initial data to the file
		//Disabled at the moment
		/*
		for(int i=0;i<initialIter;i++)
		{
			calcDiffsAndPrint(avgRate,avgVar,initialData[i].heartrate,initialData[i].variability,initialData[i].curSim);
		}
		*/



        }
        
        
        if(!baselineDone)
        {
            //Add information to the baseline data
            if(heartrate != INVALID)
            {
                avgRate += heartrate;
                baseRateCount++;
            }
            if(var != INVALID)
            {
                avgVar += var;
                baseVarCount++;
            }

		initialData[initialIter].heartrate = heartrate;
		initialData[initialIter].variability = var;
		initialData[initialIter].curSim = curSim;

		initialIter++;

        }
        else{
            calcDiffsAndPrint(avgRate,avgVar,heartrate,var,curSim);
        }
        
        //printf("Done with line.\n");
        
        //Read in the \n character from between each data line
        fgetc(file);
        
    }
    
    printf("Done parsing %s\n",argv[1]);
    
    fclose(file);
    
    if(file2 != NULL)
    {
        printf("Now parsing %s...\n",argv[MAX_ARGS - 1]);
        
        if(fgets(buffer,READMAX,file2) == NULL)
        {
            printf("Error reading comment line from file2! Aborting!\n");
            return 1;
        }
        
        while(!feof(file2))
        {
            if(fgets(buffer,READMAX,file2) == NULL)
            {
                if(feof(file2))
                    break;
                
                printf("Error reading from file2! Aborting!\n");
                return 1;
            }
        
            //printf("Read %s\n",buffer);
            
            //Parse line, extract heart rate, variability, confidence, and current simulation
            char* date;
            date = strtok(buffer,",");
            
            char* time;
            time = strtok(NULL,",");
            
            char* heartrateStr;
            heartrateStr = strtok(NULL,",");
            int heartrate = atoi(heartrateStr);
            
            char* varStr;
            varStr = strtok(NULL,",");
            int var = atoi(varStr);
            
            char* confidenceStr;
            confidenceStr = strtok(NULL,",");
            int confidence = atoi(confidenceStr);
            
            char* simStr;
            simStr = strtok(NULL,",");
            int curSim = atoi(simStr);
            
	    calcDiffsAndPrint(avgRate,avgVar,heartrate,var,curSim);

       	    //Read in the \n character from between each data line
            fgetc(file2);
        }
        
        
        
        printf("Done parsing %s!\n",argv[MAX_ARGS - 1]);
        fclose(file2);
    }
    
    printf("Completed parsing.\n");
    
    fclose(output);
    
    return 0;
}