#!/bin/bash
# Set up environment or configure limits
ulimit -n 65536
ulimit -a
# Execute the command passed as arguments to the script
exec "$@"