FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS skibuild

# Install Video Processing Libraries in FFMPEG and gcc to compile gpmf.
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
        ffmpeg \
        curl \
        procps \
        unzip \
        build-essential \
     && rm -rf /var/lib/apt/lists/* \
     && curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg     
