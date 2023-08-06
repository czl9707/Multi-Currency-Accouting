BASE_DIR = ./
HTTPS_CERTIFICATE_DIR = $(BASE_DIR)/.https_cert
SSL_CERTIFICATE_FILE = $(HTTPS_CERTIFICATE_DIR)/accountant.pem

.PHONY: all
all: run

.PHONY: cert
cert: 
	@ if [ "$(wildcard $(HTTPS_CERTIFICATE_DIR))" != "" ]; then\
		rm -rf $(HTTPS_CERTIFICATE_DIR);\
	fi
	@ mkdir $(HTTPS_CERTIFICATE_DIR)
	@ dotnet dev-certs https --trust --export-path $(SSL_CERTIFICATE_FILE) --format PEM --no-password
	@ openssl x509 -outform der -in $(SSL_CERTIFICATE_FILE) -out $(HTTPS_CERTIFICATE_DIR)/accountant.crt

.PHONY: .restore
.restore:
	dotnet restore

.PHONY: .build
.build: .restore
	dotnet build --no-restore

.PHONY: run
run: .build .trust-ssl
	dotnet run --no-build

.PHONY: .trust-ssl
.trust-ssl:
	@ cp $(SSL_CERTIFICATE_FILE) /usr/local/share/ca-certificates/accountant.crt
	@ update-ca-certificates
