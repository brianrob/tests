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

ProviderConfiguration* GetProvider(unsigned int i)
{
    ProviderConfiguration *retVal = NULL;
    if(i < MaxProviders)
    {
        retVal = &s_ProviderConfigurationList[i];
    }

    return retVal;
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
    // Parses a string with the following format:
    //
    //      ProviderName:Keywords:Level[,]*
    //
    // For example:
    //
    //      Microsoft-Windows-DotNETRuntime:0xCAFEBABE:2,Microsoft-Windows-DotNETRuntimePrivate:0xDEADBEEF:1
    //
    // Each provider configuration is separated by a ',' and each component within the configuration is
    // separated by a ':'.

    const char ProviderSeparatorChar = ',';
    const char ComponentSeparatorChar = ':';
    unsigned int len = strlen(input);
    unsigned int index = 0;
    unsigned int provConfigIndex = 0;

    while(index < len)
    {
        ProviderConfiguration *pProv = GetProvider(provConfigIndex++);
        if(pProv == NULL)
        {
            printf("NULL provider configuration.\n");
            return;
        }
        char * pCurrentChunk = &input[index];
        unsigned int currentChunkStartIndex = index;
        unsigned int currentChunkEndIndex = 0;

        // Find the next chunk.
        while(index < len && input[index] != ProviderSeparatorChar)
        {
            index++;
        }
        currentChunkEndIndex = index++;

        // Split the chunk into components.
        unsigned int chunkIndex = currentChunkStartIndex;

        // Get the provider name.
        unsigned int provNameStartIndex = chunkIndex;
        unsigned int provNameEndIndex = currentChunkEndIndex;

        while(chunkIndex < currentChunkEndIndex && input[chunkIndex] != ComponentSeparatorChar)
        {
            chunkIndex++;
        }
        provNameEndIndex = chunkIndex++;

        unsigned int provNameLen = provNameEndIndex - provNameStartIndex;
        pProv->ProviderName = malloc(provNameLen+1);
        memcpy(pProv->ProviderName, &input[provNameStartIndex], provNameLen);

        // Get the keywords.
        unsigned int keywordsStartIndex = chunkIndex;
        unsigned int keywordsEndIndex = currentChunkEndIndex;

        while(chunkIndex < currentChunkEndIndex && input[chunkIndex] != ComponentSeparatorChar)
        {
            chunkIndex++;
        }
        keywordsEndIndex = chunkIndex++;

        unsigned int keywordsLen = keywordsEndIndex - keywordsStartIndex;
        char keywords[keywordsLen+1];
        memcpy(keywords, &input[keywordsStartIndex], keywordsLen);
        keywords[keywordsLen] = '\0';
        pProv->Keywords = strtoul(keywords, NULL, 16);

        // Get the level.
        unsigned int levelStartIndex = chunkIndex;
        unsigned int levelEndIndex = currentChunkEndIndex;

        while(chunkIndex < currentChunkEndIndex && input[chunkIndex] != ComponentSeparatorChar)
        {
            chunkIndex++;
        }
        levelEndIndex = chunkIndex++;

        // Debug print the level.
        unsigned int levelLen = levelEndIndex - levelStartIndex;
        char level[levelLen+1];
        memcpy(level, &input[levelStartIndex], levelLen);
        level[levelLen] = '\0';
        pProv->Level = (unsigned short) strtoul(level, NULL, 16);
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

    for(unsigned int i=0; i<MaxProviders; i++)
    {
        PrintProvider(i);
    }

    return 0;
}


