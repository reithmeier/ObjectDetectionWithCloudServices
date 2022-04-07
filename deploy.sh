#!/bin/bash
az functionapp deployment source config-zip -g 'rg_clc3_project' -n 'clc3Functions' --src 'publish.zip'