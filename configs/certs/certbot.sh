#!/bin/sh

# Run Certbot command to obtain/renew the certificate
certbot certonly --webroot -w /var/www/certbot \
  -d ${DOMAIN} -d www.${DOMAIN} \
  --non-interactive --agree-tos --register-unsafely-without-email \
  --deploy-hook "cp /etc/letsencrypt/live/${DOMAIN}/cert.pem /data/temp/certs/cert.pem && \
                 cp /etc/letsencrypt/live/${DOMAIN}/privkey.pem /data/temp/certs/privkey.pem && \
                 cp /etc/letsencrypt/live/${DOMAIN}/fullchain.pem /data/temp/certs/fullchain.pem &&\
                 cp /etc/letsencrypt/live/${DOMAIN}/chain.pem /data/temp/certs/chain.pem"

# Ensure the flag file is created regardless of whether the certificate was renewed
touch /data/temp/certbot-complete.flag