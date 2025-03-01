# AiToys

This application provides users with functionalities to process files and data using AI models. Initially, it will leverage models based on my integrations such as [Speech-to-Text API](https://github.com/ggwozdz90/speech-to-text-api), [Translation API](https://github.com/ggwozdz90/translation-api), and [Summarization API](https://github.com/ggwozdz90/summarization-api). Unlike my other application, [Speech-to-Text CLI](https://github.com/ggwozdz90/speech-to-text-cli), which is a console application, AiToys features a user interface.

## Features

- **Transcription**: Generate SRT subtitles from audio files.
- **Translation**: Generate translated SRT subtitles from audio files.
- **Summarization**: Summarize text content using AI models.
- **Local Processing**: All processing is done locally on the user's machine, ensuring that no files are sent over the internet.

## Available Distributions

- The application is built as a self-contained executable in .NET 9 and does not require the .NET runtime to be installed. You can download the executable from the [releases page](https://github.com/ggwozdz90/ai-toys/releases).
- The application is also available as an MSIX package.
- The application leverages the [Speech-to-Text API Client](https://github.com/ggwozdz90/speech-to-text-api-client) library to handle API requests and responses.
- It communicates with the [Speech-to-Text API](https://github.com/ggwozdz90/speech-to-text-api), which performs transcription using OpenAI's Whisper model and translation using Seamless or mBART models.
- **Important**: The application requires the Speech-to-Text API to be running locally. Without the API, the application cannot function. You can run the API locally or use the [home-lab](https://github.com/ggwozdz90/home-lab) project to set it up with Docker.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Table of Contents

- [AiToys](#aitoys)
  - [Features](#features)
  - [Available Distributions](#available-distributions)
  - [License](#license)
  - [Table of Contents](#table-of-contents)
