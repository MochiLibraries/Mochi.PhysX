#!/usr/bin/env python3
import re
import subprocess

output = subprocess.check_output(["git", "submodule", "status", "external/PhysX"], encoding='utf-8').strip()

match = re.match(r"^.(?P<revision>[a-f0-9]+) external/PhysX.*", output)
assert(match is not None), f"Malformed Git output: '{output}'"

revision = match.group('revision')
print(f"::set-output name=revision::{revision}")
