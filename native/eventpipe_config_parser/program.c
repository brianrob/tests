#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct ProviderConfiguration
{
    char* ProviderName;
    unsigned long Keywords;
    unsigned short Level;
} ProviderConfiguration;

// List of provider configurations.
const unsigned int MaxProviders = 10;
ProviderConfiguration s_ProviderConfigurationList[10];

// Print the provider configuration at the specified index.
void PrintProvider(unsigned int index)
{
    ProviderConfiguration *pProv = &s_ProviderConfigurationList[index];
    printf("Name=%s, Keywords=%08lx, Level=%x\n", pProv->ProviderName, pProv->Keywords, pProv->Level);
}

// Set the provider configuration at the specified index.
void SetProvider(unsigned int index, char* provName, unsigned long keywords, unsigned short level)
{
    ProviderConfiguration *pProv = &s_ProviderConfigurationList[index];
    pProv->ProviderName = provName;
    pProv->Keywords = keywords;
    pProv->Level = level;
}

void ParseInput(char * input)
{
    unsigned int len = strlen(input);
    unsigned int index = 0;
    unsigned int provConfigIndex = 0;
    char * provName = NULL;


    while(index < len)
    {
        char * pCurrentChunk = &input[index];
        unsigned int currentChunkStartIndex = index;
        unsigned int currentChunkEndIndex = 0;

        // Find the next chunk.
        while(index <= len)
        {
            if(index < len && input[index] == ',')
            {
                currentChunkEndIndex = index;
                index++;
                break;
            }
            else if(index == len)
            {
                currentChunkEndIndex = index;
                break;
            }
            index++;
        }

        // Debug print the chunk.
        unsigned int chunkLen = currentChunkEndIndex - currentChunkStartIndex;
        char chunk[chunkLen+1];
        memcpy(chunk, pCurrentChunk, chunkLen);
        chunk[chunkLen] = '\0';
        printf("%s\n", chunk);

        // Split the chunk into components.
        unsigned int chunkIndex = currentChunkStartIndex;

        // Get the provider name.
        unsigned int provNameStartIndex = chunkIndex;
        unsigned int provNameEndIndex = currentChunkEndIndex;

        while(chunkIndex <= currentChunkEndIndex)
        {
            if(chunkIndex < currentChunkEndIndex && input[chunkIndex] == ':')
            {
                provNameEndIndex = chunkIndex;
                chunkIndex++;
                break;
            }
            else if(chunkIndex == currentChunkEndIndex)
            {
                provNameEndIndex = chunkIndex;
            }
            chunkIndex++;
        }

        // Debug print the provider name.
        unsigned int provNameLen = provNameEndIndex - provNameStartIndex;
        char provName[provNameLen+1];
        memcpy(provName, &input[provNameStartIndex], provNameLen);
        provName[provNameLen] = '\0';
        printf("\tProvName = %s\n", provName);


        // Get the keywords.
        unsigned int keywordsStartIndex = chunkIndex;
        unsigned int keywordsEndIndex = currentChunkEndIndex;

        while(chunkIndex <= currentChunkEndIndex)
        {
            if(chunkIndex < currentChunkEndIndex && input[chunkIndex] == ':')
            {
                keywordsEndIndex = chunkIndex;
                chunkIndex++;
                break;
            }
            else if(chunkIndex == currentChunkEndIndex)
            {
                keywordsEndIndex = chunkIndex;
            }
            chunkIndex++;
        }

        // Debug print the keywords.
        unsigned int keywordsLen = keywordsEndIndex - keywordsStartIndex;
        char keywords[keywordsLen+1];
        memcpy(keywords, &input[keywordsStartIndex], keywordsLen);
        keywords[keywordsLen] = '\0';
        printf("\tKeywords = %s\n", keywords);


        // Get the level.
        unsigned int levelStartIndex = chunkIndex;
        unsigned int levelEndIndex = currentChunkEndIndex;

        while(chunkIndex <= currentChunkEndIndex)
        {
            if(chunkIndex < currentChunkEndIndex && input[chunkIndex] == ':')
            {
                levelEndIndex = chunkIndex;
                chunkIndex++;
                break;
            }
            else if(chunkIndex == currentChunkEndIndex)
            {
                levelEndIndex = chunkIndex;
            }
            chunkIndex++;
        }

        // Debug print the level.
        unsigned int levelLen = levelEndIndex - levelStartIndex;
        char level[levelLen+1];
        memcpy(level, &input[levelStartIndex], levelLen);
        level[levelLen] = '\0';
        printf("\tLevel = %s\n", level);
    }
}

int main(int argc, char* argv[])
{
    if(argc <= 1)
    {
        printf("No input specified.\n");
        return 0;
    }

    ParseInput(argv[1]);

    /*for(unsigned int i=0; i<MaxProviders; i++)
    {
        PrintProvider(i);
    }*/

    return 0;
}


