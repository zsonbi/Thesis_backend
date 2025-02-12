#/bin/sh
dos2unix /configs/certs/certbot.sh;
mkdir /data/temp/certs
mkdir /data/temp/nginx
mkdir /data/temp/nginx_certs
if [ -z "$( ls -A '/data/temp/certs' )" ]; then
   echo "Empty";
   cp -r /configs/certs/*.pem /data/temp/certs/
else
   echo "Not Empty";
fi

sed -e "s/server_name[^;]*;/server_name $DOMAIN www.$DOMAIN;/g" /configs/nginx/nginx.conf > /data/temp/nginx/nginx.conf
sed -e "s/server_name[^;]*;/server_name $DOMAIN www.$DOMAIN;/g" /configs/nginx/nginx_cert.conf > /data/temp/nginx_cert/nginx.conf

rm -rf /data/temp/certbot-complete.flag;