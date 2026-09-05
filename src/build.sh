#!/usr/bin/env bash

IMAGE_NAME="${IMAGE_NAME:-ntt-bank-agent}"

docker build -t "${IMAGE_NAME}:latest" .
docker tag "${IMAGE_NAME}:latest" "jacksonveroneze/${IMAGE_NAME}"
docker push "jacksonveroneze/${IMAGE_NAME}"
