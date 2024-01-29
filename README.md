# Hashmap

## Overview

This tool was designed to analyze text files (.txt or .pdf for now) and output the most frequently used words. It provides a simple way to identify the most common words in a given text file.

## Prerequisites

- .NET SDK 8.0 or later

## Quickstart

- Clone or download the repository to your local machine.
- Open a terminal inside the project folder.
- Run `dotnet build WordCount.sln -o <output_folder_path>`.
- Inside the output folder, run `WordCount <path_to_pdf_or_txt_folder> <top>`

For example, `WordCount example.txt 10` will analyze example.txt and display the 10 most frequently used words in the file, along with the number of occurrences of each one and the total number of words that make up the text in the file.

## Key Dependencies

- PdfPig: This application uses the [PdfPig](https://github.com/UglyToad/PdfPig) package to parse and extract text from PDF files.
