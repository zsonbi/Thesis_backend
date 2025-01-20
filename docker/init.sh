#!/bin/sh
mkdir -p /scripts/initSQL
# Substitute environment variables manually using bash
sed -e "s|\${MYSQL_USER}|$MYSQL_USER|g" \
    -e "s|\${MYSQL_PASSWORD}|$MYSQL_PASSWORD|g" \
    -e "s|\${MYSQL_DATABASE}|$MYSQL_DATABASE|g" \
    /scripts/1.sql > /scripts/initSQL/1.sql
